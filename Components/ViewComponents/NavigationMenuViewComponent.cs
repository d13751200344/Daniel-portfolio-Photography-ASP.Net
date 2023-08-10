using Microsoft.AspNetCore.Mvc;
using Photography.Models;

namespace Photography.Components.ViewComponents
{
    public class NavigationMenuViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var menuItems = new List<MenuItem>
            {
                new MenuItem { Controller = "Home", Action = "Index", Label = "Home"},
                new MenuItem { Controller = "Gallery", Action = "Index", Label = "Gallery",
                        DropdownItems = new List<MenuItem>
                            { 
                                new MenuItem { Controller = "Products", Action = "Index", Label = "Product"},
                                new MenuItem { Controller = "Services", Action = "Index", Label = "Service"},
                                new MenuItem { Controller = "Foods", Action = "Index", Label = "Food"},
                                new MenuItem { Controller = "Weddings", Action = "Index", Label = "Wedding"},
                                new MenuItem { Controller = "Property", Action = "Index", Label = "Property"},

                            } },
                new MenuItem { Controller = "Courses", Action = "Index", Label = "Course"},
                new MenuItem { Controller = "Home", Action = "About", Label = "About"},
                new MenuItem { Controller = "Home", Action = "Privacy", Label = "Privacy"},
                new MenuItem { Controller = "Courses", Action = "ViewMyCart", Label = "Cart", Authorized = true },
                new MenuItem { Controller = "Courses", Action = "Orders", Label = "My Orders", Authorized = true },
                new MenuItem { Controller = "Courses", Action = "AllOrders", Label = "All Orders", Authorized = true, AllowedRoles = new List<string> { "Administrator" } },
             };

            return View(menuItems);
        }
    }
}
