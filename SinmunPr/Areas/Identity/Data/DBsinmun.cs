using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SinmunPr.Areas.Identity.Data;
using SinmunPr.Models;

namespace SinmunPr.Data
{
    public class DBsinmun : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Category> categories { get; set; }
        public DbSet<Post> posts { get; set; }
        public DbSet<MainComment> mainComments { get; set; }
        public DbSet<SubComment> subComments { get; set; }
        public DbSet<Source> sources { get; set; }
        public DbSet<Tag> tags { get; set; }
        public DbSet<PostTagId> postTagIds { get; set; }
        public DbSet<SensitiveWord> sensitiveWords { get; set; }
        public DbSet<PostIp> postIps { get; set; }
        public DbSet<AboutUs> aboutUs { get; set; }
        public DbSet<ContactUs> contactUs { get; set; }

        public DBsinmun(DbContextOptions<DBsinmun> options)
            : base(options)
        {
        }
    }
}
