namespace Pohui.Infrastructure
{
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ninject;
using Ninject.Infrastructure;
using Ninject.Modules;
using Pohui.Models;


    public class AppModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IUser>().To<UserRepository>();
            Bind<ICreative>().To<CreativeRepository>();
            Bind<IChapter>().To<ChapterRepository>();
            Bind<ITag>().To<TagRepository>();
            Bind<ILike>().To<LikeRepository>();
        }
    }
}