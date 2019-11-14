using HomeManagement.Data;
using HomeManagement.Mapper;
using HomeManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HomeManagement.API.Business
{
    public class ConfigurationSettingsService : IConfigurationSettingsService
    {
        private readonly IRepositoryFactory repositoryFactory;
        private readonly IConfigurationSettingsMapper configurationSettingsMapper;

        public ConfigurationSettingsService(IRepositoryFactory repositoryFactory,
            IConfigurationSettingsMapper configurationSettingsMapper)
        {
            this.repositoryFactory = repositoryFactory;
            this.configurationSettingsMapper = configurationSettingsMapper;
        }

        public OperationResult Delete(int id)
        {
            using (var configurationSettingsRepository = repositoryFactory.CreateConfigurationSettingsRepository())
            {
                var entity = configurationSettingsRepository.GetById(id);

                if (entity == null) return OperationResult.Error("not found");

                configurationSettingsRepository.Remove(id);

                configurationSettingsRepository.Commit();

                return OperationResult.Succeed();
            }
        }

        public ConfigurationSettingModel GetConfig(string name)
        {
            var configurationSettingsRepository = repositoryFactory.CreateConfigurationSettingsRepository();

            var entity = configurationSettingsRepository.FirstOrDefault(x => x.Name.Equals(name));

            if (entity == null) throw new NullReferenceException($"no entity found for {name}");

            return configurationSettingsMapper.ToModel(entity);
        }

        public IEnumerable<ConfigurationSettingModel> GetConfigs()
        {
            var configurationSettingsRepository = repositoryFactory.CreateConfigurationSettingsRepository();

            return configurationSettingsRepository
                    .GetAll()
                    .Select(x => configurationSettingsMapper.ToModel(x))
                    .ToList();
        }

        public OperationResult Save(ConfigurationSettingModel model)
        {
            using (var configurationSettingsRepository = repositoryFactory.CreateConfigurationSettingsRepository())
            {
                var entity = configurationSettingsRepository.GetById(model.Id);

                if (entity == null)
                {
                    entity = configurationSettingsMapper.ToEntity(model);
                    configurationSettingsRepository.Add(entity);
                }
                else
                {
                    if (!model.Name.Equals(entity.Name)) return OperationResult.Error("Config missmatch");

                    entity.Value = model.Value;
                }

                configurationSettingsRepository.Commit();

                return OperationResult.Succeed();
            }
        }
    }

    public interface IConfigurationSettingsService
    {
        IEnumerable<ConfigurationSettingModel> GetConfigs();

        ConfigurationSettingModel GetConfig(string name);

        OperationResult Save(ConfigurationSettingModel model);

        OperationResult Delete(int id);
    }
}
