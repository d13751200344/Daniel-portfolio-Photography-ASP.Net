using Microsoft.AspNetCore.Mvc;
using Photography.Data;
using Photography.Models;

namespace Photography.Components.ViewComponents
{
    public class NavigationMenuViewComponent : ViewComponent
    {

        private readonly ApplicationDbContext _dbContext;

        public NavigationMenuViewComponent(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IViewComponentResult Invoke()
        {
            var menuItems = new List<MenuItem>
            {
                new MenuItem { Controller = "Home", Action = "Index", Label = "Home"},
                new MenuItem { Controller = "Galleries", Action = "Index", Label = "Galleries", Authorized = true, AllowedRoles = new List<string> { "Administrator" }},
                new MenuItem { Controller = "Photos", Action = "Index", Label = "Photos", Authorized = true, AllowedRoles = new List<string> { "Administrator" }},
                new MenuItem { Controller = "Gallery", Action = "Index", Label = "Portfolio",
                        DropdownItems = new List<MenuItem>
                            {
                            } },
                new MenuItem { Controller = "Courses", Action = "Index", Label = "Course"},
                new MenuItem { Controller = "Home", Action = "About", Label = "About"},
                new MenuItem { Controller = "Home", Action = "Privacy", Label = "Privacy"},
                new MenuItem { Controller = "Courses", Action = "ViewMyCart", Label = "Cart", Authorized = true },
                new MenuItem { Controller = "Courses", Action = "Orders", Label = "My Orders", Authorized = true },
                new MenuItem { Controller = "Courses", Action = "AllOrders", Label = "All Orders", Authorized = true, AllowedRoles = new List<string> { "Administrator" } },
            };

            var galleries = _dbContext.Gallery.ToList(); // Retrieve the list of galleries

            // Create menu items for each gallery
            foreach (var gallery in galleries)
            {
                menuItems[3].DropdownItems.Add(new MenuItem
                {
                    Controller = "Galleries", // Set the appropriate controller
                    Action = "Index",      // Set the appropriate action
                    Label = gallery.GalleryName,
                });
            }

            // ... add the remaining menu items ...

            return View(menuItems);
        }

        /*
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
        */
    }
}
