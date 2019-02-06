using System;
using System.Collections.Generic;
using System.Text;

namespace PizzaOffer.DomainClasses
{
    public interface IBaseEntity
    {

    }
    public abstract class BaseEntityWithDates : IBaseEntity
    {
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset? UpdatedDate { get; set; }
    }
    public abstract class BaseEntityWithKey<TKey> : BaseEntityWithDates
    {
        public TKey Id { get; set; }
    }

    public abstract class BaseEntity : BaseEntityWithKey<int>
    {

    }
}
