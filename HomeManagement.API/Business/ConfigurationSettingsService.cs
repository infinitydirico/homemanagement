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
        private readonly IConfigurationSettingsRepository configurationSettingsRepository;
        private readonly IConfigurationSettingsMapper configurationSettingsMapper;

        public ConfigurationSettingsService(IConfigurationSettingsRepository configurationSettingsRepository,
            IConfigurationSettingsMapper configurationSettingsMapper)
        {
            this.configurationSettingsRepository = configurationSettingsRepository;
            this.configurationSettingsMapper = configurationSettingsMapper;
        }

        public OperationResult Delete(int id)
        {
            var entity = configurationSettingsRepository.GetById(id);

            if (entity == null) return OperationResult.Error("not found");

            configurationSettingsRepository.Remove(id);

            configurationSettingsRepository.Commit();

            return OperationResult.Succeed();
        }

        public ConfigurationSettingModel GetConfig(string name)
        {
            var entity = configurationSettingsRepository.FirstOrDefault(x => x.Name.Equals(name));

            if (entity == null) throw new NullReferenceException($"no entity found for {name}");

            return configurationSettingsMapper.ToModel(entity);
        }

        public IEnumerable<ConfigurationSettingModel> GetConfigs()
        {
            return configurationSettingsRepository
                .GetAll()
                .Select(x => configurationSettingsMapper.ToModel(x))
                .ToList();
        }

        public OperationResult Save(ConfigurationSettingModel model)
        {
            var entity = configurationSettingsRepository.GetById(model.Id);

            if(entity == null)
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

    public interface IConfigurationSettingsService
    {
        IEnumerable<ConfigurationSettingModel> GetConfigs();

        ConfigurationSettingModel GetConfig(string name);

        OperationResult Save(ConfigurationSettingModel model);

        OperationResult Delete(int id);
    }
}
