using System;
using System.Collections.Generic;
using System.Text;

namespace PizzaOffer.DomainClasses
{
    public interface IBaseEntity
    {

    }

    public abstract class BaseEntity<TKey> : IBaseEntity
    {
        public TKey Id { get; set; }
        // Todo: Add CreatedDate and UpdatedDate here and config them to automatically get value on add and on update
    }

    public abstract class BaseEntity : BaseEntity<int>
    {

    }
}
