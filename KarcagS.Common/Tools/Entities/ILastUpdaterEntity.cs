using System;

namespace KarcagS.Common.Tools.Entities;

public interface ILastUpdaterEntity<TKey>
{
    TKey LastUpdaterId { get; set; }
}