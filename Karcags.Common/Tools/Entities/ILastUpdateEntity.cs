using System;

namespace Karcags.Common.Tools.Entities;

public interface ILastUpdateEntity
{
    DateTime LastUpdate { get; set; }
}