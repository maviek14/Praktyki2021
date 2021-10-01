using System;
using System.ComponentModel.DataAnnotations;

namespace EDService.ViewModels
{
    public class AddedDateGroup
    {
        [DataType(DataType.Date)]
        public DateTime? AddedTime { get; set; }

        public int UserCount { get; set; }
    }
}