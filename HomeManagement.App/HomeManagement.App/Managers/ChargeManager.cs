using Autofac;
using HomeManagement.App.Data;
using HomeManagement.App.Data.Entities;
using HomeManagement.App.Services.Rest;
using HomeManagement.Core.Caching;
using HomeManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeManagement.App.Managers
{
    public interface IChargeManager
    {
        int PageCount { get; }

        int TotalPages { get; }

        int CurrentPage { get; }

        Task<IEnumerable<Charge>> Load(int accountId);

        Task<IEnumerable<Charge>> NextPage();

        Task<IEnumerable<Charge>> PreviousPage();

        Task AddChargeAsync(Charge charge);

        Task DeleteChargeAsync(Charge charge);
    }

    public class ChargeManager : IChargeManager
    {
        private readonly IChargeServiceClient chargeServiceClient = App._container.Resolve<IChargeServiceClient>();
        private readonly GenericRepository<Charge> chargeRepository = new GenericRepository<Charge>();
        private readonly ICachingService cachingService = App._container.Resolve<ICachingService>();
        //private IEnumerable<Charge> charges = new List<Charge>();

        private ChargePageModel page = new ChargePageModel
        {
            PageCount = 6,
            CurrentPage = 1
        };

        public int PageCount => page.PageCount;

        public int TotalPages => page.TotalPages;

        public int CurrentPage => page.CurrentPage;

        public async Task AddChargeAsync(Charge charge)
        {
            await chargeServiceClient.Post(charge);
            cachingService.StoreOrUpdate("ForceApiCall", true);
        }

        public async Task DeleteChargeAsync(Charge charge)
        {
            await chargeServiceClient.Delete(charge.Id);
            cachingService.StoreOrUpdate("ForceApiCall", true);
        }

        public async Task<IEnumerable<Charge>> Load(int accountId)
        {
            page.AccountId = accountId;

            return await Paginate();
        }

        public async Task<IEnumerable<Charge>> NextPage()
        {
            if (page.CurrentPage.Equals(page.TotalPages))
            {
                var skip = (page.CurrentPage - 1) * page.PageCount;
                return GetCachedFilteredCharges(skip);
            }

            page.CurrentPage++;

            return await Paginate();
        }

        public async Task<IEnumerable<Charge>> PreviousPage()
        {
            if (page.CurrentPage == 1)
            {
                var skip = (page.CurrentPage - 1) * page.PageCount;
                return GetCachedFilteredCharges(skip);
            }

            page.CurrentPage--;

            return await Paginate();
        }

        private async Task<IEnumerable<Charge>> Paginate()
        {
            if (!cachingService.Get<bool>("ForceApiCall"))
            {
                var skip = (page.CurrentPage - 1) * page.PageCount;

                if (chargeRepository.Count(x => x.AccountId.Equals(page.AccountId)) > skip)
                {
                    var c = GetCachedFilteredCharges(skip);
                    return await Task.FromResult(c);
                }
            }

            page.Charges.Clear();

            await Task.Delay(500);

            page = await chargeServiceClient.Page(page);

            var chargesResult = from charge in page.Charges
                                select new Charge
                                {
                                    Id = charge.Id,
                                    AccountId = charge.AccountId,
                                    CategoryId = charge.CategoryId,
                                    ChargeType = (ChargeType)Enum.Parse(typeof(ChargeType), charge.ChargeType.ToString()),
                                    Date = charge.Date,
                                    Name = charge.Name,
                                    Price = charge.Price,
                                    ChangeStamp = DateTime.Now,
                                    LastApiCall = DateTime.Now,
                                    NeedsUpdate = false
                                };

            UpdateCachedCharges(chargesResult);

            return chargesResult;
        }

        private void UpdateCachedCharges(IEnumerable<Charge> charges)
        {
            Task.Run(() =>
            {
                foreach (var item in charges)
                {
                    if (!chargeRepository.Any(x => x.Id.Equals(item.Id)))
                    {
                        chargeRepository.Add(item);
                    }
                }
                chargeRepository.Commit();
            });
        }

        private IEnumerable<Charge> GetCachedFilteredCharges(int skip)
            => chargeRepository
            .Where(x => x.AccountId.Equals(page.AccountId))
            .OrderByDescending(x => x.Id)
            .Skip(skip)
            .Take(page.PageCount)
            .ToList();
    }
}
