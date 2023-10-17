﻿using Azure;
using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Net.Http.Headers;
using Tickets.API.Helpers;
using Tickets.API.Models.DTO.Categoria;
using Tickets.API.Models.DTO.Ticket;
using Tickets.API.Repositories.Implementation;
using Tickets.API.Repositories.Interface;

namespace Tickets.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly ITicketRepository ticketRepository;

        public TicketController(ITicketRepository ticketRepository)
        {
            this.ticketRepository = ticketRepository;
        }

        [HttpPost("UploadFile"), DisableRequestSizeLimit]
        [Authorize]
        public async Task<IActionResult> UploadFile()
        {
            //var response = await categoriaRepository.CreateAsync(request);
            var ticketArchivoDtos = new List<string>();
            var formCollection = await Request.ReadFormAsync();
            var files = formCollection.Files;
            var Name = "";
            var Extension = "";
            //var folderName = Path.Combine("Resources");
            var tempFolderName = Path.Combine("Temp");
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), tempFolderName);


            if (files.Any(f => f.Length == 0))
            {
                return BadRequest();
            }

            var x = 1;
            foreach (var file in files)
            {
                var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                var fileNameSplit = fileName.Split(".");

                //si no tiene extension
                if(fileNameSplit.Length == 1)
                {
                    ModelState.AddModelError("error", "El archivo no tiene extensión valida.");
                    return ValidationProblem(ModelState);
                }

                if (
                    fileNameSplit[fileNameSplit.Length - 1].ToUpper() != "jpg".ToUpper() &&
                    fileNameSplit[fileNameSplit.Length - 1].ToUpper() != "pdf".ToUpper() &&
                    fileNameSplit[fileNameSplit.Length - 1].ToUpper() != "png".ToUpper() &&
                    fileNameSplit[fileNameSplit.Length - 1].ToUpper() != "psd".ToUpper() &&
                    fileNameSplit[fileNameSplit.Length - 1].ToUpper() != "zip".ToUpper()

                    )
                {
                    ModelState.AddModelError("error", "Solo se aceptan archivos JPG, PDF, PNG, PSD y ZIP");
                    return ValidationProblem(ModelState);
                }

                Name = Guid.NewGuid().ToString();
                Extension = fileNameSplit[fileNameSplit.Length - 1].ToUpper();

                var fullPath = Path.Combine(pathToSave, Name + "." + Extension);
                var dbPath = Path.Combine(tempFolderName, Name + "." + Extension); //you can add this path to a list and then return all dbPaths to the client if require
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
                ticketArchivoDtos.Add(Name + "." + Extension);


            }
            return Ok(ticketArchivoDtos);
        }

        [HttpPost("CrearTicket")]
        [Authorize]
        public async Task<IActionResult> CrearTicket(CrearTicketRequestDto ticketRequestDto)
        {
            var response = await ticketRepository.CrearTicket(ticketRequestDto, Guid.Parse(User.GetId()));
            
            if (!response.response)
            {
                ModelState.AddModelError("error", response.message);
                return ValidationProblem(ModelState);
            }

            return Ok(response.result);
        }

        [HttpGet("GetUsuarioTickets")]
        [Authorize]
        public async Task<IActionResult> GetUsuarioTickets([FromQuery] TicketByEstatusDto model)
        {
            var response = await ticketRepository.GetUsuarioTickets(model.estatusId, Guid.Parse(User.GetId()));

            if (!response.response)
            {
                ModelState.AddModelError("error", response.message);
                return ValidationProblem(ModelState);
            }

            return Ok(response.result);
        }

        [HttpGet("GetSupervisorTickets")]
        [Authorize(Roles = "Supervisor")]
        public async Task<IActionResult> GetSupervisorTickets([FromQuery] TicketByEstatusDto model)
        {
            var response = await ticketRepository.GetSupervisorTickets(model.estatusId,Guid.Parse(User.GetId()));

            if (!response.response)
            {
                ModelState.AddModelError("error", response.message);
                return ValidationProblem(ModelState);
            }

            return Ok(response.result);
        }
    }
}
