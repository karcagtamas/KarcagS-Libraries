using System;

namespace Karcags.Common.Tools.Entities;

public interface ILastUpdaterEntity<TKey>
{
    TKey LastUpdaterId { get; set; }
}