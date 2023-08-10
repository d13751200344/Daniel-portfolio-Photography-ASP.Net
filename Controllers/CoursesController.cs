using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Photography.Data;
using Photography.Models;
using Stripe;
using Stripe.Checkout;

namespace Photography.Controllers
{
    public class CoursesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private IConfiguration _configuration;  //property for the parameter that will be passed in


        public CoursesController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;  //get the parameter that is passed here
        }

        // GET: Courses
        public async Task<IActionResult> Index()
        {
              return _context.Courses != null ? 
                          View(await _context.Courses
                                .OrderBy(course => course.CourseName)
                                .ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Courses'  is null.");
        }

        // GET: Courses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Courses == null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }


        // GET: Courses/Create
        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CourseName,CourseDescription,Price")] Course course)
        {
            if (ModelState.IsValid)
            {
                _context.Add(course);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }

        // GET: Courses/Edit/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Courses == null)
            {
                return NotFound();
            }

            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            return View(course);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CourseName,CourseDescription,Price")] Course course)
        {
            if (id != course.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(course);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(course.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }

        // GET: Courses/Delete/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Courses == null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // POST: Courses/Delete/5
        [Authorize(Roles = "Administrator")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Courses == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Courses'  is null.");
            }
            var course = await _context.Courses.FindAsync(id);
            if (course != null)
            {
                _context.Courses.Remove(course);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CourseExists(int id)
        {
          return (_context.Courses?.Any(e => e.Id == id)).GetValueOrDefault();
        }



        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddToCart(int courseId) //courseId passed from <form>
        {   // Enable users to add products to carts

            // Get logged-in user's id; if no such a user, assign null
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Attempt to get the specific user's active cart
            var cart = await _context.Carts
                .FirstOrDefaultAsync(cart => cart.UserId == userId && cart.Active == true);
            // if the user don't have a cart, we create one
            if (cart == null)
            {
                cart = new Cart { UserId = userId };
                await _context.AddAsync(cart);  //add new cart to database (prepare SQL statement)
                await _context.SaveChangesAsync();  //execute the SQL statement
            }

            // find the course
            var course = await _context.Courses
                .FirstOrDefaultAsync(course => course.Id == courseId);

            // if no product
            if (course == null)
            {
                return NotFound();  //404 page
            }

            // create a cartItem object
            var cartItem = new Models.CartItem
            {
                Cart = cart,
                Course = course,
                Price = course.Price,
            };

            // If valid, do all the goodness (see if we made a stable model or not)
            if (ModelState.IsValid)
            {
                await _context.AddAsync(cartItem);  //add new cart to database (prepare SQL statement)
                await _context.SaveChangesAsync();  //execute the SQL statement
                return RedirectToAction("ViewMyCart");  //redirect users to ViewMyCart.cshtml
            }
            // otherwise GTFO
            return NotFound();
        }



        [Authorize]
        public async Task<IActionResult> ViewMyCart()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var cart = await _context.Carts
                .Include(cart => cart.User)   //access cart.User
                .Include(cart => cart.CartItems)   //access cart.CartItems
                .ThenInclude(cartItem => cartItem.Course)  //joining the Course table to each of the cart items
                .FirstOrDefaultAsync(cart => cart.UserId == userId && cart.Active == true);

            return View(cart);
        }



        [HttpPost]
        [Authorize]
        public async Task<IActionResult> DeleteCartItem(int cartItemId) //in the DeleteCartItem form, name = "cartItemId"
        {
            // Get logged in user, if no such a user, assign null
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Attempt to get the specific user's active cart
            var cart = await _context.Carts
                .FirstOrDefaultAsync(cart => cart.UserId == userId && cart.Active == true);

            if (cart == null) return NotFound();

            var cartItem = await _context.CartItems
                .FirstOrDefaultAsync(cartItem => cartItem.Cart == cart && cartItem.Id == cartItemId);

            if (cartItem != null)
            {
                _context.CartItems.Remove(cartItem);	// prepare sql statement
                await _context.SaveChangesAsync();	// execute it

                return RedirectToAction("ViewMyCart");
            }

            return NotFound();
        }




        [Authorize]
        public async Task<IActionResult> Checkout()
        {
            // Get logged in user, if no such a user, assign null
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var cart = await _context.Carts
                .Include(cart => cart.User)  //access cart.User
                .Include(cart => cart.CartItems)  //access cart.CartItems
                .ThenInclude(cartItem => cartItem.Course)  //joining the Course tables to each of the cart items
                .FirstOrDefaultAsync(cart => cart.UserId == userId && cart.Active == true);
            // Attempt to get the specific user's active cart

            //make a new Order object
            var order = new Models.Order
            {
                UserId = userId,
                Cart = cart,
                Total = cart.CartItems.Sum(cartItem => cartItem.Price),
                ShippingAddress = "",
                PaymentMethod = Models.PaymentMethods.VISA,
            };

            //get the enum valuse list in "Order.cs" and assign it to ViewData["PaymentMethods"]
            //this will be passed to Checkout view
            ViewData["PaymentMethods"] = new SelectList(Enum.GetValues(typeof(PaymentMethods)));

            // return the order object to CheckOut.cshtml
            return View(order);
        }



        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Payment(string shippingAddress, PaymentMethods paymentMethod)
        {
            //get userID
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            //get cart data in database > includes cartItems > find the right cart(userId & active)
            var cart = await _context.Carts
                .Include(cart => cart.CartItems)
                .FirstOrDefaultAsync(cart => cart.UserId == userId && cart.Active == true);

            if (cart == null) return NotFound();

            //Add order data to the session
            HttpContext.Session.SetString("ShippingAddress", shippingAddress);
            HttpContext.Session.SetString("PaymentMethod", paymentMethod.ToString());

            //Set Stripe API key
            StripeConfiguration.ApiKey = _configuration.GetSection("Stripe")["SecretKey"];

            // Create our Stripe options
            var options = new Stripe.Checkout.SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions> {
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long)(cart.CartItems.Sum(cartItem => cartItem.Price) * 100),
                            Currency = "cad",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = "Photography Course Purchase",
                            },
                        },
                        Quantity = 1,
                    },
                },
                PaymentMethodTypes = new List<string>
                {
                    "card"
                },
                Mode = "payment",
                SuccessUrl = "https://" + Request.Host + "/Courses/SaveOrder",
                CancelUrl = "https://" + Request.Host + "/Courses/ViewMyCart",
            };

            //use the session service that Stripe offers
            var service = new Stripe.Checkout.SessionService();
            Stripe.Checkout.Session session = service.Create(options);

            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
        }




        //Save the order and show it once users have done their purchasing
        public async Task<IActionResult> SaveOrder()
        {
            //get userID
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            //get cart data in database > includes cartItems > find the right cart(userId & active)
            var cart = await _context.Carts
                .Include(cart => cart.CartItems)
                .FirstOrDefaultAsync(cart => cart.UserId == userId && cart.Active == true);

            //get our data out of the session
            var paymentMethod = HttpContext.Session.GetString("PaymentMethod");
            var shippingAddress = HttpContext.Session.GetString("ShippingAddress");

            var order = new Order
            {
                UserId = userId,
                Cart = cart,
                Total = cart.CartItems.Sum(cartItem => cartItem.Price),
                ShippingAddress = shippingAddress,
                PaymentMethod = (PaymentMethods)Enum.Parse(typeof(PaymentMethods), paymentMethod),
                PaymentReceived = true,
            };

            //store data of order in database
            await _context.AddAsync(order);
            await _context.SaveChangesAsync();

            //After storage, make cart.Active = false because it has been checked out
            cart.Active = false;
            _context.Update(cart);
            await _context.SaveChangesAsync();

            //Then redirect users to OrderDetails page and pass the order.Id as an argument
            return RedirectToAction("OrderDetails", new { id = order.Id });
            // this will cooperate with the method below
        }



        // OrderDetails method
        [Authorize]
        public async Task<IActionResult> OrderDetails(int id)
        {
            // Get logged in user, if no such a user, assign null
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var order = await _context.Orders
                .Include(order => order.User)  //loads the related User entity associated with the order.
                .Include(order => order.Cart)
                .ThenInclude(cart => cart.CartItems)
                .ThenInclude(cartItem => cartItem.Course)
                .FirstOrDefaultAsync(order => order.UserId == userId && order.Id == id);

            if (order == null) return NotFound();

            return View(order);
        }



        [Authorize]
        public async Task<IActionResult> Orders()
        {
            // Get logged in user, if no such a user, assign null
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var orders = await _context.Orders
                .OrderByDescending(order => order.Id)
                .Where(order => order.UserId == userId)
                .ToListAsync();

            if (orders == null) return NotFound();

            return View(orders);
        }
    }
}
