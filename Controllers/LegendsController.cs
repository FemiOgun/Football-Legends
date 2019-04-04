using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FootieLegends.Models;
using PagedList;


namespace FootieLegends.Controllers
{
    public class LegendsController : Controller
    {
        private FootballLegendsDBEntities db = new FootballLegendsDBEntities();

        // GET: Legends
        public ActionResult Index(string sortOrder, string currentFilter, string searchName, int? page)
        {

            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";

            if (searchName != null)
            {
                page = 1;
            }
            else
            {
                searchName = currentFilter;
            }

            ViewBag.CurrentFilter = searchName;


            var players = from s in db.Legends
                          select s;

            if (!String.IsNullOrEmpty(searchName))
            {
                //if 'a' contains any letters from the searchName(Artist Name) then return searched artists in view.
                players = players.Where(s => s.Name.Contains(searchName));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    players = players.OrderByDescending(s => s.Name);
                    break;


                default:
                    players = players.OrderBy(s => s.Name);
                    break;
            }

            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(players.ToPagedList(pageNumber, pageSize));
        }

        // GET: Legends/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Legend legend = db.Legends.Find(id);
            if (legend == null)
            {
                return HttpNotFound();
            }
            return View(legend);
        }

        // GET: Legends/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Legends/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Nationality,CareerStartYear,CareerEndYear,Position,Clubs,Appearances,Goals,ClubHonors,InternationalHonors,BallondOrs,Description,Image")] Legend legend)
        {


            if (legend.Image == null)
            {
                legend.Image = "https://cdn.pixabay.com/photo/2018/09/30/22/12/silhouette-3714836_960_720.png";
            }


            if (ModelState.IsValid)
            {
                db.Legends.Add(legend);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(legend);
        }

        // GET: Legends/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Legend legend = db.Legends.Find(id);
            if (legend == null)
            {
                return HttpNotFound();
            }
            return View(legend);
        }

        // POST: Legends/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Nationality,CareerStartYear,CareerEndYear,Position,Clubs,Appearances,Goals,ClubHonors,InternationalHonors,BallondOrs,Description,Image")] Legend legend)
        {

            if (legend.Image == null)
            {
                legend.Image = "https://cdn.pixabay.com/photo/2018/09/30/22/12/silhouette-3714836_960_720.png";
            }


            if (ModelState.IsValid)
            {
                db.Entry(legend).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(legend);
        }

        // GET: Legends/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Legend legend = db.Legends.Find(id);
            if (legend == null)
            {
                return HttpNotFound();
            }
            return View(legend);
        }

        // POST: Legends/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Legend legend = db.Legends.Find(id);
            db.Legends.Remove(legend);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
