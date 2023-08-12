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
                new MenuItem { Controller = "Portfolio", Action = "Index", Label = "Portfolio",
                        DropdownItems = new List<MenuItem>
                            {
                            } },
                //new MenuItem { Controller = "Photos", Action = "Index", Label = "Photos", Authorized = true, AllowedRoles = new List<string> { "Administrator" }},
                new MenuItem { Controller = "Courses", Action = "Index", Label = "Course"},
                //new MenuItem { Controller = "Home", Action = "Contact", Label = "Contact"},
                new MenuItem { Controller = "Home", Action = "Privacy", Label = "Privacy"},
                new MenuItem { Controller = "Courses", Action = "ViewMyCart", Label = "Cart", Authorized = true },
                new MenuItem { Controller = "Courses", Action = "Orders", Label = "My Orders", Authorized = true },
                new MenuItem { Controller = "Courses", Action = "AllOrders", Label = "All Orders", Authorized = true, AllowedRoles = new List<string> { "Administrator" } },
            };


            var galleries = _dbContext.Gallery.ToList(); // Retrieve the list of galleries

            // Create menu items for each gallery
            foreach (Gallery gallery in galleries)
            {
                menuItems[2].DropdownItems.Add(new MenuItem
                {
                    Controller = "Portfolio",
                    Action = "Details",
                    RouteValues = new Dictionary<string, string> { { "id", gallery.Id.ToString() } }, // Use the gallery index+1 as the id
                    Label = gallery.GalleryName,
                });
            }

            return View(menuItems);
        }
    }
}
