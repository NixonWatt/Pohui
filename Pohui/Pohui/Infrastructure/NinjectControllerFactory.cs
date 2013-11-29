using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Ninject;
using Ninject.Web.Common;
using Pohui.Infrastructure;
using Pohui.Models;


namespace Pohui.Infrastructure
{
    public class NinjectControllerFactory : DefaultControllerFactory
    {
        private IKernel ninjectKernel;

        public NinjectControllerFactory()
        {
            ninjectKernel = new StandardKernel();
            AddBindings();
        }

        private void AddBindings()
        {
            ninjectKernel.Bind<IUser>().To<UserRepository>();
            ninjectKernel.Bind<ICreative>().To<CreativeRepository>();
            ninjectKernel.Bind<IChapter>().To<ChapterRepository>();
            ninjectKernel.Bind<ITag>().To<TagRepository>();
            ninjectKernel.Bind<ILike>().To<LikeRepository>();
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            return controllerType == null
                       ? null
                       : (IController)ninjectKernel.Get(controllerType);
        }
    }
}