﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Microbrewit.Model;
using Microbrewit.Api.Controllers;
using Microbrewit.Repository;
using Microbrewit.Api.Automapper;
using System.IO;
using Newtonsoft.Json;
using log4net;
using System.Web.Http;
using System.Net;
using System.Web.Http.Results;
using Microbrewit.Model.DTOs;

namespace Microbrewit.Test
{
    [TestFixture]
    public class OtherControllerTest
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IOtherRepository _repository;
        private MicrobrewitContext _context;
        private const string JSONPATH = @"..\..\JSON\";

        [TestFixtureSetUp]
        public void Init()
        {
            TestUtil.DeleteDataInDatabase();
            AutoMapperConfiguration.Configure();
            _context = new MicrobrewitContext();
        }

        [TestFixtureTearDown]
        public void TearDown()
        {
            _context.Dispose();
        }
       
        [Test]
        public void GetAllOthersReturnsEverythingInRepository()
        {
            _repository = new OtherTestRepository();
            var controller = new OtherController(_repository);
            var others = controller.GetOthers();
            Assert.AreEqual(others.Others.Count, 3);
        }

        [Test]
        public void GetAllOthersFromDataBaseReturnsNotNull()
        {
            _repository = new OtherRepository(new MicrobrewitContext());
            var controller = new OtherController(_repository);
            var others = controller.GetOthers();
            Assert.NotNull(others);
        }

        [Test]
        public void PostOtherToDatabaseGetsAdded()
        {
            using (var file = new StreamReader(JSONPATH + "other.json"))
            {
                _repository = new OtherRepository(new MicrobrewitContext());
                string jsonString = file.ReadToEnd();
                var count = _repository.GetAll().Count();
                var others = JsonConvert.DeserializeObject<List<OtherDto>>(jsonString);

                var controller = new OtherController(_repository);
                controller.PostOther(others);
                var result = controller.GetOthers();
                Assert.AreEqual(count + others.Count, result.Others.Count);
            }
        }

        [Test]
        public void PutOtherGetsChanged()
        {
            _repository = new OtherRepository(new MicrobrewitContext());
            
            var controller = new OtherController(_repository);
            var other = _context.Others.FirstOrDefault();
            Log.DebugFormat("other.Name= {0}", other.Name);
            other.Name = "YELLOW";
            var statusCode = controller.PutOther(other.Id, other);
            var updatedOther = _context.Others.SingleOrDefault(o => o.Id == other.Id);
            Assert.AreEqual(other.Name, updatedOther.Name);
        }




    }
}