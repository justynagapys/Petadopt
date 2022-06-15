using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Petadopt.Models;
using System.IO;
using Azure.Storage.Queues;

namespace Petadopt.Controllers
{
    public class PetsController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        private readonly petadoptContext _context = new petadoptContext();

        /*public PetsController(petadoptContext context)
        {
            _context = context;
        }*/

        public PetsController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Pets
        public async Task<IActionResult> Index()
        {
            var pets = await _context.Pets.ToListAsync();
            var petsWithPhotos = from p in pets
                                 select new Pet
                                 {
                                     Id = p.Id,
                                     Picture = getBlobAsBase64(p.Picture),
                                     Name = p.Name,
                                     Type = p.Type,
                                     Age = p.Age,
                                     Gender = p.Gender,
                                     Size = p.Size,
                                     Coat = p.Coat,
                                     Description = p.Description
                                 };
              return View(petsWithPhotos);
        }

        // GET: Pets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Pets == null)
            {
                return NotFound();
            }

            var pet = await _context.Pets
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pet == null)
            {
                return NotFound();
            }

            return View(pet);
        }

        // GET: Pets/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Pets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Type,Age,Gender,Size,Coat,Description,Picture")] Pet pet)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pet);
                await _context.SaveChangesAsync();

                //Do kolejki
                var clientAccessKey = "DefaultEndpointsProtocol=https;AccountName=storagepetadopt;AccountKey=ZYgb1jpti/Nj3FA2kj1xsoWhRa4GPRIHxj92uTv1Fqo+gl2u7FVP6CiSNJzxevVi/DNmiZTAY5WH+ASt8OL1gQ==;EndpointSuffix=core.windows.net";
                var client = new QueueServiceClient(clientAccessKey);
                //client.CreateQueue("pets"); //create queue
                var newpetsQueue = client.GetQueueClient("newpet");
                await newpetsQueue.SendMessageAsync(pet.Name + " is looking for a new home!", timeToLive: new TimeSpan(3, 0, 0, 0, 0));                

                return RedirectToAction(nameof(Index));
            }
            return View(pet);
        }

        // GET: Pets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Pets == null)
            {
                return NotFound();
            }

            var pet = await _context.Pets.FindAsync(id);
            if (pet == null)
            {
                return NotFound();
            }
            return View(pet);
        }

        // POST: Pets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Type,Age,Gender,Size,Coat,Description,Picture")] Pet pet)
        {
            if (id != pet.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pet);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PetExists(pet.Id))
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
            return View(pet);
        }

        // GET: Pets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Pets == null)
            {
                return NotFound();
            }

            var pet = await _context.Pets
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pet == null)
            {
                return NotFound();
            }

            return View(pet);
        }

        // POST: Pets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Pets == null)
            {
                return Problem("Entity set 'petadoptContext.Pets'  is null.");
            }
            var pet = await _context.Pets.FindAsync(id);
            if (pet != null)
            {
                _context.Pets.Remove(pet);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PetExists(int id)
        {
          return (_context.Pets?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private string getBlobAsBase64(string id)
        {
            var clientAccessKey = "DefaultEndpointsProtocol=https;AccountName=storagepetadopt;AccountKey=ZYgb1jpti/Nj3FA2kj1xsoWhRa4GPRIHxj92uTv1Fqo+gl2u7FVP6CiSNJzxevVi/DNmiZTAY5WH+ASt8OL1gQ==;EndpointSuffix=core.windows.net";
            var client = new BlobServiceClient(clientAccessKey);

            var imagesContainer = client.GetBlobContainerClient("images");
            var blob = imagesContainer.GetBlobClient(id);

            if (!blob.Exists())
                return id;

            var blobMimeType = blob.GetProperties().Value.ContentType;

            var imageBinary = blob.DownloadContent().Value.Content.ToArray();
            var imageBase64 = Convert.ToBase64String(imageBinary);

            return imageBase64;
        }

        [HttpGet, ActionName("BlobBase64")]
        public async Task<IActionResult> GetBlobBase64()
        {
            var clientAccessKey = "DefaultEndpointsProtocol=https;AccountName=storagepetadopt;AccountKey=ZYgb1jpti/Nj3FA2kj1xsoWhRa4GPRIHxj92uTv1Fqo+gl2u7FVP6CiSNJzxevVi/DNmiZTAY5WH+ASt8OL1gQ==;EndpointSuffix=core.windows.net";
            var client = new BlobServiceClient(clientAccessKey);

            var imagesContainer = client.GetBlobContainerClient("images");
            var blob = imagesContainer.GetBlobClient("rex.png");

            var blobMimeType = blob.GetProperties().Value.ContentType;

            var imageBinary = blob.DownloadContent().Value.Content.ToArray();
            var imageBase64 = Convert.ToBase64String(imageBinary);

            return Content(imageBase64);

        }

        [HttpGet, ActionName("Blob")]
        public async Task<IActionResult> GetBlob() //pets/blob
        {
            var clientAccessKey = "DefaultEndpointsProtocol=https;AccountName=storagepetadopt;AccountKey=ZYgb1jpti/Nj3FA2kj1xsoWhRa4GPRIHxj92uTv1Fqo+gl2u7FVP6CiSNJzxevVi/DNmiZTAY5WH+ASt8OL1gQ==;EndpointSuffix=core.windows.net";
            var client = new BlobServiceClient(clientAccessKey);

            var imagesContainer = client.GetBlobContainerClient("images");
            var blob = imagesContainer.GetBlobClient("rex.png");

            var blobMimeType = blob.GetProperties().Value.ContentType; //type of blob

            var imageBinary = blob.DownloadContent().Value.Content.ToArray();
            var imageBase64 = Convert.ToBase64String(imageBinary);

            return File(blob.DownloadContent().Value.Content.ToArray(), blobMimeType); //return type with the file to browser

            //var serverRootPath = _webHostEnvironment.ContentRootPath;
            //var blobSavePath = Path.Combine(serverRootPath, "AppData", blob.Name);

            //save
            //var streamWrite = System.IO.File.OpenWrite(blobSavePath); //open file
            //blob.DownloadTo(streamWrite);
            //streamWrite.Dispose();

            //Read
            //var streamRead = System.IO.File.OpenRead(blobSavePath);
            //return File(streamRead, blobMimeType);

            //return Content(blobSavePath);

        }

        [HttpGet, ActionName("SendBlob")]
        public async Task<IActionResult> PostBlob(string image) //movies/SendBlob?image=rex.png
        {
            var clientAccessKey = "DefaultEndpointsProtocol=https;AccountName=storagepetadopt;AccountKey=ZYgb1jpti/Nj3FA2kj1xsoWhRa4GPRIHxj92uTv1Fqo+gl2u7FVP6CiSNJzxevVi/DNmiZTAY5WH+ASt8OL1gQ==;EndpointSuffix=core.windows.net";
            var client = new BlobServiceClient(clientAccessKey);
            var imagesContainer = client.GetBlobContainerClient("images");
            var blob = imagesContainer.GetBlobClient(image);

            blob.DeleteIfExists();
            var imgFullPath = Path.Combine(_webHostEnvironment.ContentRootPath, "AppData", image);
            await blob.UploadAsync(imgFullPath);

            return Content(image + " został przesłany");
        }
    }
}
