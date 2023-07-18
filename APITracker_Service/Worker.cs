using APITracker_Service.Application.Interfaces;
using APITracker_Service.Entities;
using APITracker_Service.Enums;
using APITracker_Service.Repositories;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace APITracker_Service
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IEmail _email;
        private readonly IHttpService _httpservice;
        private readonly IHostApplicationLifetime _applicationLifetime;
        private readonly IEnderecoApiRepository _enderecoApiRepository;

        public Worker(
            ILogger<Worker> logger,
            IEmail email,
            IHttpService httpservice,
            IHostApplicationLifetime applicationLifetime,
            IEnderecoApiRepository enderecoApiRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _email = email ?? throw new ArgumentNullException(nameof(email));
            _httpservice = httpservice ?? throw new ArgumentNullException(nameof(httpservice));
            _applicationLifetime = applicationLifetime ?? throw new ArgumentNullException(nameof(applicationLifetime));
            _enderecoApiRepository = enderecoApiRepository;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                //_logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                //var status = await _httpservice.Check("https://w.google.com");
                //if (status != System.Net.HttpStatusCode.OK)
                //{
                //    _email.SendEmail("yuri.lightbase@sebraemg.com.br", "Worker Service", $"E-mail enviado: {DateTimeOffset.Now}");
                //    _applicationLifetime.StopApplication();
                //}

                await Realtime();


                await Task.Delay(30000, stoppingToken);
            }
        }

        //private async Task Realtime()
        //{
        //    try
        //    {
        //        foreach (EnderecoAPI endereco in await _enderecoApiRepository.BuscarTodos())
        //        {
        //            try
        //            {
        //                using HttpClient client = new();

        //                client.Timeout = endereco.TimeOutEmMinutos <= 0 ? TimeSpan.FromSeconds(20) : TimeSpan.FromMinutes(endereco.TimeOutEmMinutos);

        //                Stopwatch stopwatch = new();
        //                stopwatch.Start();

        //                HttpResponseMessage response = null;

        //                if (endereco.Method.Equals(Method.POST))
        //                {
        //                    StringContent json = new(
        //                        content: endereco.Body,
        //                        encoding: Encoding.UTF8,
        //                        mediaType: "application/json");

        //                    response = client.PostAsync(endereco.Endereco, json).Result;
        //                }
        //                else if (endereco.Method.Equals(Method.GET))
        //                {
        //                    response = client.GetAsync(endereco.Endereco).Result;
        //                }

        //                stopwatch.Stop();

        //                TimeSpan duration = stopwatch.Elapsed;

        //                HttpStatusCode statusCode = response.StatusCode;

        //                string error = string.Empty;

        //                if (statusCode == HttpStatusCode.BadRequest || statusCode == HttpStatusCode.InternalServerError)
        //                {
        //                    error = response.Content.ReadAsStringAsync().Result;
        //                }

        //                endereco.StatusCode = (int)statusCode;
        //                endereco.TimeOutEmMinutos = duration.Seconds;
        //                endereco.Error = string.IsNullOrEmpty(error) ? string.Empty : error;
        //            }
        //            catch (Exception ex)
        //            {
        //                endereco.StatusCode = 500;
        //                endereco.Error = ex.Message;
        //                _email.SendEmail("yuri.lightbase@sebraemg.com.br", "Worker Service", $"E-mail enviado: {DateTimeOffset.Now + ex.Message}");
        //                _applicationLifetime.StopApplication();
        //            }
        //            await _enderecoApiRepository.Alterar(endereco);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}

        private async Task Realtime()
        {
            try
            {
                foreach (EnderecoAPI endereco in await _enderecoApiRepository.BuscarTodos())
                {
                    try
                    {
                        using HttpClient client = new();

                        client.Timeout = endereco.TimeOutEmMinutos <= 0 ? TimeSpan.FromSeconds(20) : TimeSpan.FromMinutes(endereco.TimeOutEmMinutos);

                        Stopwatch stopwatch = Stopwatch.StartNew();

                        HttpResponseMessage response = null;

                        if (endereco.Method.Equals(Method.POST))
                        {
                            StringContent json = new(
                                content: endereco.Body ?? string.Empty,
                                encoding: Encoding.UTF8,
                                mediaType: "application/json");

                            response = await client.PostAsync(endereco.Endereco, json);
                        }
                        else if (endereco.Method.Equals(Method.GET))
                        {
                            response = await client.GetAsync(endereco.Endereco);
                        }

                        stopwatch.Stop();

                        TimeSpan duration = stopwatch.Elapsed;

                        HttpStatusCode statusCode = response.StatusCode;

                        string error = string.Empty;

                        if (statusCode == HttpStatusCode.BadRequest || statusCode == HttpStatusCode.InternalServerError)
                        {
                            error = await response.Content.ReadAsStringAsync();
                        }

                        endereco.StatusCode = (int)statusCode;
                        endereco.TimeOutEmMinutos = (int)duration.TotalSeconds;
                        endereco.Error = string.IsNullOrEmpty(error) ? string.Empty : error;

                        await _enderecoApiRepository.Alterar(endereco);
                    }
                    catch (Exception ex)
                    {
                        endereco.StatusCode = 500;
                        endereco.Error = ex.Message;
                        _email.SendEmail("yuri.lightbase@sebraemg.com.br", "Worker Service", $"E-mail enviado: {DateTimeOffset.Now + ex.Message}");
                        _applicationLifetime.StopApplication();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}