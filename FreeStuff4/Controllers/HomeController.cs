using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FreeStuff4.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using FreeStuff4.Data;

namespace FreeStuff4.Controllers
{

    public class HomeController : Controller
    {
        private string _conStr = "Data Source=.\\sqlexpress;Initial Catalog=FreeStuff;Integrated Security=True";


        public IActionResult Index()
        {
            IndexViewModel vm = new IndexViewModel();
            FreeStuffDb db = new FreeStuffDb(_conStr);
            List<int> ids = HttpContext.Session.Get<List<int>>("ListingIds");
            IEnumerable<Post> posts = db.GetAllPosts();
            vm.Posts = posts.Select(p => new PostViewModel
            {
                Post = p,
                CanDelete = ids != null && ids.Contains(p.Id)
            });
            return View(vm);
        }
        public IActionResult AddPostForm()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddPost(Post p)
        {
            FreeStuffDb db = new FreeStuffDb(_conStr);
            db.AddPost(p);

            List<int> ids = HttpContext.Session.Get<List<int>>("ListingIds");
            if(ids == null)
            {
                ids = new List<int>();
            }
            ids.Add(p.Id);
            HttpContext.Session.Set("ListingIds", ids);

            return Redirect("/Home/Index");
        }
        [HttpPost]
        public IActionResult DeletePost(int id)
        {
            FreeStuffDb db = new FreeStuffDb(_conStr);
            db.DeletePost(id);
            return Redirect("/Home/Index");
        }

    }
    public static class SessionExtensions
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T Get<T>(this ISession session, string key)
        {
            string value = session.GetString(key);

            return value == null ? default(T) :
                JsonConvert.DeserializeObject<T>(value);
        }
    }
}
