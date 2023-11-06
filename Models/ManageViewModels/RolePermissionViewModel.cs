using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bfws.Models.ManageViewModels
{
    public class RolePermissionViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool Selected { get; set; }
    }
}
