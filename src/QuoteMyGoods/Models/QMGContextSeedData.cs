using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace QuoteMyGoods.Models
{
    public class QMGContextSeedData
    {
        private QMGContext _context;
        private UserManager<QMGUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        private ILogger<QMGContextSeedData> _logger;

        private IdentityRole adminRole;
        private IdentityRole plebRole;

        public QMGContextSeedData(QMGContext context, UserManager<QMGUser> userManager, ILogger<QMGContextSeedData> logger, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
            _roleManager = roleManager;
        }

        /*
         * Gets a hashed string
         */
        private string GetHash(SHA1 sha1, string input)
        {
            byte[] data = sha1.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();
            for(int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }

        public async Task EnsureSeedDataAsync()
        {
            /*
            var a = await _roleManager.FindByNameAsync("Administrator");
            var b = await _roleManager.FindByNameAsync("Pleb");
            try
            {
                var c = await _userManager.FindByEmailAsync("noalgalex@gmail.com");
            }catch(AggregateException e)
            {
                Console.Write(e.ToString());
            }*/
            //create the admin role
            if(await _roleManager.FindByNameAsync("Administrator") == null)
            {
                adminRole = new IdentityRole { Name = "Administrator" };

                await _roleManager.CreateAsync(adminRole);
            }

            //create the pleb role
            if(await _roleManager.FindByNameAsync("Pleb") == null)
            {
                plebRole = new IdentityRole { Name = "Pleb" };

                await _roleManager.CreateAsync(plebRole);
            }

            //creat user noalgalex
            if(await _userManager.FindByEmailAsync("noalgalex@gmail.com") == null)
            {
                var hash = SHA1.Create();
                var user = new QMGUser()
                {
                    UserName = "noalgalex",
                    Email = "noalgalex@gmail.com"
                };

                //get the admin role
                var adminRole = await _roleManager.FindByNameAsync("Administrator");
                //create a new identity role
                var identityRole = new IdentityUserRole<string>()
                {
                    RoleId = adminRole.Id,
                    UserId = user.Id
                };

                //add role to user
                user.Roles.Add(identityRole);

                //create a new claim
                var jtdClaim = new IdentityUserClaim<string>()
                {
                    ClaimType = "JoinTheDots",
                    ClaimValue = GetHash(hash, "noalgalex")
                };

                //add to user
                user.Claims.Add(jtdClaim);

                //save user to db
                var createdUser =  await _userManager.CreateAsync(user, "Passw0rd!");

                if (!createdUser.Succeeded)
                {
                    _logger.LogError("failed to add user");
                }
            }

            if(await _userManager.FindByEmailAsync("alexlogan@alexlogan.io") == null)
            {
                var hash = SHA1.Create();
                var plebUser = new QMGUser()
                {
                    UserName = "alexlogan",
                    Email = "alexlogan@alexlogan.io"
                };

                var plebRole = await _roleManager.FindByNameAsync("Pleb");
                plebUser.Roles.Add(new IdentityUserRole<string>()
                {
                    RoleId = plebRole.Id,
                    UserId = plebUser.Id
                });

                var jtdClaim = new IdentityUserClaim<string>()
                {
                    ClaimType = "JoinTheDots",
                    ClaimValue = GetHash(hash, "alexlogan")
                };

                plebUser.Claims.Add(jtdClaim);

                var createdPlebUser = await _userManager.CreateAsync(plebUser, "Passw0rd!");

                if (!createdPlebUser.Succeeded)
                {
                    _logger.LogError("failed to add user");
                }
            }

            if (!_context.Products.Any())
            {
                var someTimber = new Product()
                {
                    Name = "Oak Plank",
                    Category = "Timber",
                    Description = "An oak plank of timber",
                    Price = 16.0m,
                    ImgUrl = "http://www.birbek.com/gfx/products/large/l_oldtradition.jpg"
                };

                _context.Products.Add(someTimber);

                var ashTimber = new Product()
                {
                    Name = "Ash Plank",
                    Category = "Timber",
                    Description = "An ash plank of timber",
                    Price = 4.0m,
                    ImgUrl = "http://www.countrymouldings.com/images/butcher-block-countertops/prefinished-ash-plank-countertop-m.jpg"
                };

                _context.Products.Add(ashTimber);

                var mahoganyTimber = new Product()
                {
                    Name = "Mahogany Plank",
                    Category = "Timber",
                    Description = "An mahogany plank of timber",
                    Price = 8.0m,
                    ImgUrl = "http://www.countrymouldings.com/images/butcher-block-countertops/prefinished-mahogany-plank-countertop-m.jpg"
                };

                _context.Add(mahoganyTimber);

                var nails = new Product()
                {
                    Name = "Nails",
                    Category = "General",
                    Description = "Some nails",
                    Price = 9.0m,
                    ImgUrl = "http://s7g3.scene7.com/is/image/ae235/cat840028?$catImageSmall$"
                };

                _context.Add(nails);

                var screws = new Product()
                {
                    Name = "Screws",
                    Category = "General",
                    Description = "Some screws",
                    Price = 2.0m,
                    ImgUrl = "http://screwcapsuk.com/_userfiles/pages/images/supachipscrews/SupaChipScrews.jpg"
                };

                _context.Add(screws);

                var tinRoof = new Product()
                {
                    Name = "Tin Roof",
                    Category = "Roofing",
                    Description = "A section of tin roof",
                    Price = 15.0m,
                    ImgUrl = "http://img04.deviantart.net/cc5e/a/large/textures/tmetal/tin_roof.jpg"
                };

                _context.Add(tinRoof);

                var thatchRoof = new Product()
                {
                    Name = "Thatch Roof",
                    Category = "Roofing",
                    Description = "A nice section of thatch roof",
                    Price = 130.0m,
                    ImgUrl = "http://area.autodesk.com/userdata/forum/9/9893_vQXnLqo0rA5WdSOSzrZv.jpg"
                };

                _context.Add(thatchRoof);

                var sheetMetal = new Product()
                {
                    Name = "Sheet Metal",
                    Category = "Metal",
                    Description = "Its not that bad.",
                    Price = 50.0m,
                    ImgUrl = "http://travisperkins.scene7.com/is/image/travisperkins/R2362_116692_00?$normal$"
                };

                _context.Add(sheetMetal);

                var hammer = new Product()
                {
                    Name = "Hammer",
                    Category = "Tools",
                    Description = "A hammer.",
                    Price = 7.0m,
                    ImgUrl = "http://pngimg.com/upload/hammer_PNG3890.png"
                };

                _context.Add(hammer);

                var drill = new Product()
                {
                    Name = "Drill",
                    Category = "Tools",
                    Description = "Power drill",
                    Price = 11.0m,
                    ImgUrl = "http://www.staticwhich.co.uk/media/images/taster/electric-drill-taster-334372.jpg"
                };

                _context.Add(drill);

                var pliars = new Product()
                {
                    Name = "Pliars",
                    Category = "Tools",
                    Description = "A pair of pliars.",
                    Price = 12.0m,
                    ImgUrl = "https://upload.wikimedia.org/wikipedia/commons/5/51/Tool-pliers.jpg"
                };

                _context.Add(pliars);

                var saw = new Product()
                {
                    Name = "Saw",
                    Category = "Tools",
                    Description = "A saw.",
                    Price = 18.0m,
                    ImgUrl = "http://www.harborfreight.com/media/catalog/product/cache/1/image/9df78eab33525d08d6e5fb8d27136e95/i/m/image_14661.jpg"
                };

                _context.Add(saw);

                var spiritLevel = new Product()
                {
                    Name = "Spirit Level",
                    Category = "Tools",
                    Description = "A spirit level.",
                    Price = 12.0m,
                    ImgUrl = "http://travisperkins.scene7.com/is/image/travisperkins/T3274_167337_00?id=8y9TX3&fmt=jpg&fit=constrain,1&wid=310&hei=310"
                };

                _context.Add(spiritLevel);

                var hackSaw = new Product()
                {
                    Name = "Hack Saw",
                    Category = "Tools",
                    Description = "A hack saw.",
                    Price = 18.0m,
                    ImgUrl = "https://www.dlsweb.rmit.edu.au/toolbox/electrotech/toolbox1204/resources/03workshop/05hand_tools/images/hacksaw.jpg"
                };

                _context.Add(hackSaw);

                var philipsHeadScrewdriver = new Product()
                {
                    Name = "Philips head screwdriver",
                    Category = "Tools",
                    Description = "A philips head screwdriver.",
                    Price = 9.0m,
                    ImgUrl = "http://images.ffx.co.uk/tools/STA064930.JPG"
                };

                _context.Add(philipsHeadScrewdriver);

                var flatHeadScrewdriver = new Product()
                {
                    Name = "Flat Head Screwdriver",
                    Category = "Tools",
                    Description = "A flat head screwdriver.",
                    Price = 8.0m,
                    ImgUrl = "http://previewcf.turbosquid.com/Preview/2014/05/22__23_40_00/screwdriver_overview.jpg7F96BB1B-6EDF-4DFB-A9B22D7DD2801BB6.jpgLarger.jpg"
                };

                _context.Add(flatHeadScrewdriver);

                var dowlingRod = new Product()
                {
                    Name = "Dowling Rod",
                    Category = "DIY",
                    Description = "A dowling rod.",
                    Price = 1800.0m,
                    ImgUrl = "https://placebrandingofpublicspace.files.wordpress.com/2013/02/dowel.jpg?w=645"
                };

                _context.Add(dowlingRod);

                var superGlue = new Product()
                {
                    Name = "Super Glue",
                    Category = "DIY",
                    Description = "Suuper glue",
                    Price = 1.0m,
                    ImgUrl = "http://www.pratleyadhesives.com.au/media/1020/pratley-superglue-20g_500x409.jpg"
                };

                _context.Add(superGlue);

                var redBricks = new Product()
                {
                    Name = "Red Brick",
                    Category = "Materials",
                    Description = "Red brick",
                    Price = 5.0m,
                    ImgUrl = "http://thumbs.dreamstime.com/t/single-red-brick-white-background-isolate-45246307.jpg"
                };

                _context.Add(redBricks);

                var insulatingBricks = new Product()
                {
                    Name = "Insulating Bricks",
                    Category = "Materials",
                    Description = "Insulating bricks",
                    Price = 4.0m,
                    ImgUrl = "http://www.fieldfurnace.com.au/images/008.JPG"
                };

                _context.Add(insulatingBricks);

                var axe = new Product()
                {
                    Name = "Axe",
                    Category = "Tools",
                    Description = "A axe.",
                    Price = 36.0m,
                    ImgUrl = "https://www.raymears.com/_rm_pictures_/Gransfors-Outdoor-Axe1.jpg"
                };

                _context.Add(axe);

                var angleGrinder = new Product()
                {
                    Name = "Angle Grinder",
                    Category = "Tools",
                    Description = "An angle grinder.",
                    Price = 181.0m,
                    ImgUrl = "http://assets.jewson.co.uk/category-images/9624/Main/9624.jpg"
                };

                _context.Add(angleGrinder);

                var smartMaterials = new Product()
                {
                    Name = "Smart Materials",
                    Category = "Materials",
                    Description = "Some smart material",
                    Price = 18.0m,
                    ImgUrl = "https://webdocs.cs.ualberta.ca/~database/MEMS/sma_mems/img/goop.gif"
                };

                _context.Add(smartMaterials);

                var nut = new Product()
                {
                    Name = "Nut",
                    Category = "DIY",
                    Description = "A nut.",
                    Price = 18000.0m,
                    ImgUrl = "https://www.belmetric.com/images/AML14SS.jpg"
                };

                _context.Add(nut);

                _context.SaveChanges();
            }
        }
    }
}
