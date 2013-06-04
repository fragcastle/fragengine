using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Reflection;
using FragEd.Data;
using FragEngine.Data;
using FragEngine.Entities;

namespace FragEd.Controllers
{
    public class ProjectController : IController<Project>
    {

        public Project Project { get; set; }

        public void Create()
        {
            // create a new project
            Project = new Project();


        }

        public void Read()
        {
            throw new NotImplementedException();
        }

        public void Update()
        {
            throw new NotImplementedException();
        }

        public void Delete()
        {
            throw new NotImplementedException();
        }
    }   
}
