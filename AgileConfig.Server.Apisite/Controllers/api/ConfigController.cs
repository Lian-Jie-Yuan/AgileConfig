﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AgileConfig.Server.Apisite.Filters;
using AgileConfig.Server.Apisite.Models;
using AgileConfig.Server.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AgileConfig.Server.Apisite.Controllers.api
{
    [TypeFilter(typeof(BasicAuthenticationAttribute))]
    [Route("api/[controller]")]
    public class ConfigController : Controller
    {
        private readonly IConfigService _configService;
        private readonly IAppService _appService;

        public ConfigController(
            IConfigService configService,
            ILoggerFactory loggerFactory,
            IAppService appService)
        {
            _configService = configService;
            _appService = appService;
        }
        // GET: api/<controller>
        [HttpGet("app/{appId}")]
        public async Task<ActionResult<List<ConfigVM>>> Get(string appId)
        {
            var app =await _appService.GetAsync(appId);
            if (!app.Enabled)
            {
                return NotFound();
            }

            var configs = await _configService.GetPublishedConfigsByAppId(appId);
            var vms = configs.Select(c => {
                return new ConfigVM() {
                    Id = c.Id,
                    AppId = c.AppId,
                    Group = c.Group,
                    Key = c.Key,
                    Value = c.Value,
                    Status = c.Status
                };
            });

            return vms.ToList();
        }
       
    }
}
