using System;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Plugin.Widgets.NivoSlider.Infrastructure.Cache;
using Nop.Plugin.Widgets.NivoSlider.Models;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Media;
using Nop.Web.Framework.Components;

namespace Nop.Plugin.Widgets.NivoSlider.Components
{
    [ViewComponent(Name = "WidgetsNivoSlider")]
    public class WidgetsNivoSliderViewComponent : NopViewComponent
    {
        private readonly IStoreContext _storeContext;
        private readonly IStaticCacheManager _cacheManager;
        private readonly ISettingService _settingService;
        private readonly IPictureService _pictureService;
        private readonly IWebHelper _webHelper;
        private readonly NivoSliderSettings _nivoSliderSettings;
        private readonly IWorkContext _workContext;
        private readonly ILocalizationService _localizationService;

        public WidgetsNivoSliderViewComponent(IStoreContext storeContext, 
            IStaticCacheManager cacheManager, 
            ISettingService settingService, 
            IPictureService pictureService,
            IWebHelper webHelper,
            NivoSliderSettings nivoSliderSettings,
            IWorkContext workContext,
            ILocalizationService localizationService)
        {
            _storeContext = storeContext;
            _cacheManager = cacheManager;
            _settingService = settingService;
            _pictureService = pictureService;
            _webHelper = webHelper;
            _nivoSliderSettings = nivoSliderSettings;
            _workContext = workContext;
            _localizationService = localizationService;
        }

        public IViewComponentResult Invoke(string widgetZone, object additionalData)
        {
            var storeId = _storeContext.CurrentStore.Id;
            var languageId = _workContext.WorkingLanguage.Id;
            var nivoSliderSettings = _settingService.LoadSetting<NivoSliderSettings>(storeId);            
            
            var picture1Id = nivoSliderSettings.Picture1Id == "" ? 0 : int.Parse(_localizationService.GetLocalizedSetting(nivoSliderSettings, (NivoSliderSettings x) => x.Picture1Id, languageId, storeId, true, true));
            var picture2Id = nivoSliderSettings.Picture2Id == "" ? 0 : int.Parse(_localizationService.GetLocalizedSetting(nivoSliderSettings, (NivoSliderSettings x) => x.Picture2Id, languageId, storeId, true, true));
            var picture3Id = nivoSliderSettings.Picture3Id == "" ? 0 : int.Parse(_localizationService.GetLocalizedSetting(nivoSliderSettings, (NivoSliderSettings x) => x.Picture3Id, languageId, storeId, true, true));
            var picture4Id = nivoSliderSettings.Picture4Id == "" ? 0 : int.Parse(_localizationService.GetLocalizedSetting(nivoSliderSettings, (NivoSliderSettings x) => x.Picture4Id, languageId, storeId, true, true));
            var picture5Id = nivoSliderSettings.Picture5Id == "" ? 0 : int.Parse(_localizationService.GetLocalizedSetting(nivoSliderSettings, (NivoSliderSettings x) => x.Picture5Id, languageId, storeId, true, true));


            var model = new PublicInfoModel
            {
                Picture1Url = GetPictureUrl(picture1Id),
                Text1 = _localizationService.GetLocalizedSetting(nivoSliderSettings, (NivoSliderSettings x) => x.Text1, languageId, storeId, true, true),
                Link1 = _localizationService.GetLocalizedSetting(nivoSliderSettings, (NivoSliderSettings x) => x.Link1, languageId, storeId, true, true),
                AltText1 = _localizationService.GetLocalizedSetting(nivoSliderSettings, (NivoSliderSettings x) => x.AltText1, languageId, storeId, true, true),

                Picture2Url = GetPictureUrl(picture2Id),
                Text2 = _localizationService.GetLocalizedSetting(nivoSliderSettings, (NivoSliderSettings x) => x.Text2, languageId, storeId, true, true),
                Link2 = _localizationService.GetLocalizedSetting(nivoSliderSettings, (NivoSliderSettings x) => x.Link2, languageId, storeId, true, true),
                AltText2 = _localizationService.GetLocalizedSetting(nivoSliderSettings, (NivoSliderSettings x) => x.AltText2, languageId, storeId, true, true),

                Picture3Url = GetPictureUrl(picture3Id),
                Text3 = _localizationService.GetLocalizedSetting(nivoSliderSettings, (NivoSliderSettings x) => x.Text3, languageId, storeId, true, true),
                Link3 = _localizationService.GetLocalizedSetting(nivoSliderSettings, (NivoSliderSettings x) => x.Link3, languageId, storeId, true, true),
                AltText3 = _localizationService.GetLocalizedSetting(nivoSliderSettings, (NivoSliderSettings x) => x.AltText3, languageId, storeId, true, true),

                Picture4Url = GetPictureUrl(picture4Id),
                Text4 = _localizationService.GetLocalizedSetting(nivoSliderSettings, (NivoSliderSettings x) => x.Text4, languageId, storeId, true, true),
                Link4 = _localizationService.GetLocalizedSetting(nivoSliderSettings, (NivoSliderSettings x) => x.Link4, languageId, storeId, true, true),
                AltText4 = _localizationService.GetLocalizedSetting(nivoSliderSettings, (NivoSliderSettings x) => x.AltText4, languageId, storeId, true, true),

                Picture5Url = GetPictureUrl(picture5Id),
                Text5 = _localizationService.GetLocalizedSetting(nivoSliderSettings, (NivoSliderSettings x) => x.Text5, languageId, storeId, true, true),
                Link5 = _localizationService.GetLocalizedSetting(nivoSliderSettings, (NivoSliderSettings x) => x.Link5, languageId, storeId, true, true),
                AltText5 = _localizationService.GetLocalizedSetting(nivoSliderSettings, (NivoSliderSettings x) => x.AltText5, languageId, storeId, true, true),
            };

            if (string.IsNullOrEmpty(model.Picture1Url) && string.IsNullOrEmpty(model.Picture2Url) &&
                string.IsNullOrEmpty(model.Picture3Url) && string.IsNullOrEmpty(model.Picture4Url) &&
                string.IsNullOrEmpty(model.Picture5Url))
                //no pictures uploaded
                return Content("");

            return View("~/Plugins/Widgets.NivoSlider/Views/PublicInfo.cshtml", model);
        }

        protected string GetPictureUrl(int pictureId)
        {
            var cacheKey = string.Format(ModelCacheEventConsumer.PICTURE_URL_MODEL_KEY, 
                pictureId, _webHelper.IsCurrentConnectionSecured() ? Uri.UriSchemeHttps : Uri.UriSchemeHttp);

            return _cacheManager.Get(cacheKey, () =>
            {
                //little hack here. nulls aren't cacheable so set it to ""
                var url = _pictureService.GetPictureUrl(pictureId, showDefaultPicture: false) ?? "";
                return url;
            });
        }
    }
}
