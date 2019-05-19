using Autofac;
using HomeManagement.App.Data.Entities;
using HomeManagement.App.Services.Rest;
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
    }

    public class ChargeManager : IChargeManager
    {
        private readonly IChargeServiceClient chargeServiceClient = App._container.Resolve<IChargeServiceClient>();

        private IEnumerable<Charge> charges = new List<Charge>();

        private ChargePageModel page = new ChargePageModel
        {
            PageCount = 6,
            CurrentPage = 1
        };

        public int PageCount => page.PageCount;

        public int TotalPages => page.TotalPages;

        public int CurrentPage => page.CurrentPage;

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

                return charges.Skip(skip).Take(page.PageCount).ToList();
            }

            page.CurrentPage++;

            return await Paginate();
        }

        public async Task<IEnumerable<Charge>> PreviousPage()
        {
            if (page.CurrentPage == 1)
            {
                var skip = (page.CurrentPage - 1) * page.PageCount;

                return charges.Skip(skip).Take(page.PageCount).ToList();
            }

            page.CurrentPage--;

            return await Paginate();
        }

        private async Task<IEnumerable<Charge>> Paginate()
        {
            var chargesCount = charges.Count();

            var skip = (page.CurrentPage - 1) * page.PageCount;

            if(chargesCount > skip)
            {
                var c = charges.Skip(skip).Take(page.PageCount).ToList();
                return await Task.FromResult(c);
            }

            page.Charges.Clear();

            page = await chargeServiceClient.Page(page);

            var chargesResult = from charge in page.Charges select new Charge
            {
                Id = charge.Id,
                AccountId = charge.AccountId,
                CategoryId = charge.CategoryId,
                ChargeType = (ChargeType)Enum.Parse(typeof(ChargeType),charge.ChargeType.ToString()),
                Date = charge.Date,
                Name = charge.Name,
                Price = charge.Price
            };

            ((List<Charge>)charges).AddRange(chargesResult);

            return chargesResult;
        }
    }
}
