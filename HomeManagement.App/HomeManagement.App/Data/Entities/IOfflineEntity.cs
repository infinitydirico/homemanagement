using System;

namespace HomeManagement.App.Data.Entities
{
    public interface IOfflineEntity
    {
        DateTime ChangeStamp { get; set; }

        DateTime LastApiCall { get; set; }
    }
}
