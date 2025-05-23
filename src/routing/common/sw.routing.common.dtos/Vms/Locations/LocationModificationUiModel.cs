﻿using System.ComponentModel.DataAnnotations;

namespace sw.routing.common.dtos.Vms.Locations
{
    public class LocationModificationUiModel
    {
        [Required(AllowEmptyStrings = false)]
        [Editable(true)]
        public string ModifiedName { get; set; }

        [Key]
        public long Id { get; set; }

        public object Message { get; set; }
    }
}