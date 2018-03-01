using cloudscribe.Pagination.Models;
using cloudscribe.Web.Pagination;
using dotnetnepal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnetnepal.ViewModels
{
    public class ProjectListViewModel
    {
        public ProjectListViewModel()
        {
            ProjectRepos = new PagedResult<ProjectRepo>();
            Projects = new PagedResult<Project>();
            Contributors = new List<ProjectContributor>();
        }


        public string Q { get; set; }
        public string Type { get; set; }
        public ProjectRepoSummary Summary { get; set; }
        public PagedResult<ProjectRepo> ProjectRepos { get; set; }
        public PagedResult<Project> Projects { get; set; }
        public List<ProjectContributor> Contributors { get; set; }

        public ProjectContributor GetContributor(string name)
        {
            foreach(var c in Contributors)
            {
                if (c.Name == name) return c;
            }

            return null;
        }

        
    }
}
