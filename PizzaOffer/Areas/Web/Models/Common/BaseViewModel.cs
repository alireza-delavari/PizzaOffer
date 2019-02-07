using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaOffer.Areas.Web.Models.Common
{
    public interface IBaseViewModel
    {

    }

    public class BaseViewModelWithDates : IBaseViewModel
    {
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset? UpdatedDate { get; set; }
    }

    public class BaseViewModelWithKey<TKey> : BaseViewModelWithDates
    {
        public TKey Id { get; set; }
    }

    public class BaseViewModel : BaseViewModelWithKey<int>
    {

    }
}
