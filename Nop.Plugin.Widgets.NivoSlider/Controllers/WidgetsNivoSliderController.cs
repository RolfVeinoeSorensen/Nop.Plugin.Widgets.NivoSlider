using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Plugin.Widgets.NivoSlider.Models;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Media;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;

namespace Nop.Plugin.Widgets.NivoSlider.Controllers
{
    [Area(AreaNames.Admin)]
    public class WidgetsNivoSliderController : BasePluginController
    {
        private readonly ILocalizationService _localizationService;
        private readonly ILanguageService _languageService;
        private readonly INotificationService _notificationService;
        private readonly IPermissionService _permissionService;
        private readonly IPictureService _pictureService;
        private readonly ISettingService _settingService;
        private readonly IStoreContext _storeContext;

        public WidgetsNivoSliderController(ILocalizationService localizationService,
            ILanguageService languageService,
            INotificationService notificationService,
            IPermissionService permissionService, 
            IPictureService pictureService,
            ISettingService settingService,
            IStoreContext storeContext)
        {
            _localizationService = localizationService;
            _languageService = languageService;
            _notificationService = notificationService;
            _permissionService = permissionService;
            _pictureService = pictureService;
            _settingService = settingService;
            _storeContext = storeContext;
        }

        public IActionResult Configure()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageWidgets))
                return AccessDeniedView();

            //load settings for a chosen store scope
            var storeScope = _storeContext.ActiveStoreScopeConfiguration;
            var nivoSliderSettings = _settingService.LoadSetting<NivoSliderSettings>(storeScope);

            var pic1Id = 0;
            var pic2Id = 0;
            var pic3Id = 0;
            var pic4Id = 0;
            var pic5Id = 0;
            int.TryParse(nivoSliderSettings.Picture1Id.ToString(), out pic1Id);
            int.TryParse(nivoSliderSettings.Picture2Id.ToString(), out pic2Id);
            int.TryParse(nivoSliderSettings.Picture3Id.ToString(), out pic3Id);
            int.TryParse(nivoSliderSettings.Picture4Id.ToString(), out pic4Id);
            int.TryParse(nivoSliderSettings.Picture5Id.ToString(), out pic5Id);

            var model = new ConfigurationModel
            {
                Picture1Id = pic1Id,
                Text1 = nivoSliderSettings.Text1,
                Link1 = nivoSliderSettings.Link1,
                AltText1 = nivoSliderSettings.AltText1,
                Picture2Id = pic2Id,
                Text2 = nivoSliderSettings.Text2,
                Link2 = nivoSliderSettings.Link2,
                AltText2 = nivoSliderSettings.AltText2,
                Picture3Id = pic3Id,
                Text3 = nivoSliderSettings.Text3,
                Link3 = nivoSliderSettings.Link3,
                AltText3 = nivoSliderSettings.AltText3,
                Picture4Id = pic4Id,
                Text4 = nivoSliderSettings.Text4,
                Link4 = nivoSliderSettings.Link4,
                AltText4 = nivoSliderSettings.AltText4,
                Picture5Id = pic5Id,
                Text5 = nivoSliderSettings.Text5,
                Link5 = nivoSliderSettings.Link5,
                AltText5 = nivoSliderSettings.AltText5,
                ActiveStoreScopeConfiguration = storeScope
            };

            if (storeScope > 0)
            {
                model.Picture1Id_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Picture1Id, storeScope);
                model.Text1_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Text1, storeScope);
                model.Link1_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Link1, storeScope);
                model.AltText1_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.AltText1, storeScope);
                model.Picture2Id_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Picture2Id, storeScope);
                model.Text2_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Text2, storeScope);
                model.Link2_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Link2, storeScope);
                model.AltText2_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.AltText2, storeScope);
                model.Picture3Id_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Picture3Id, storeScope);
                model.Text3_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Text3, storeScope);
                model.Link3_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Link3, storeScope);
                model.AltText3_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.AltText3, storeScope);
                model.Picture4Id_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Picture4Id, storeScope);
                model.Text4_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Text4, storeScope);
                model.Link4_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Link4, storeScope);
                model.AltText4_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.AltText4, storeScope);
                model.Picture5Id_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Picture5Id, storeScope);
                model.Text5_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Text5, storeScope);
                model.Link5_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Link5, storeScope);
                model.AltText5_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.AltText5, storeScope);
            }
            AddLocales(_languageService, model.Locales, delegate (ConfigurationModel.ConfigurationLocalizedModel locale, int languageId)
            {
                int.TryParse(_localizationService.GetLocalizedSetting(nivoSliderSettings, (NivoSliderSettings x) => x.Picture1Id, languageId, storeScope, false, false), out pic1Id);
                int.TryParse(_localizationService.GetLocalizedSetting(nivoSliderSettings, (NivoSliderSettings x) => x.Picture2Id, languageId, storeScope, false, false), out pic2Id);
                int.TryParse(_localizationService.GetLocalizedSetting(nivoSliderSettings, (NivoSliderSettings x) => x.Picture3Id, languageId, storeScope, false, false), out pic3Id);
                int.TryParse(_localizationService.GetLocalizedSetting(nivoSliderSettings, (NivoSliderSettings x) => x.Picture4Id, languageId, storeScope, false, false), out pic4Id);
                int.TryParse(_localizationService.GetLocalizedSetting(nivoSliderSettings, (NivoSliderSettings x) => x.Picture5Id, languageId, storeScope, false, false), out pic5Id);

                locale.Picture1Id = pic1Id;
                locale.Text1 = _localizationService.GetLocalizedSetting(nivoSliderSettings, (NivoSliderSettings x) => x.Text1, languageId, storeScope, false, false);
                locale.Link1 = _localizationService.GetLocalizedSetting(nivoSliderSettings, (NivoSliderSettings x) => x.Link1, languageId, storeScope, false, false);
                locale.AltText1 = _localizationService.GetLocalizedSetting(nivoSliderSettings, (NivoSliderSettings x) => x.AltText1, languageId, storeScope, false, false);

                locale.Picture2Id = pic2Id;
                locale.Text2 = _localizationService.GetLocalizedSetting(nivoSliderSettings, (NivoSliderSettings x) => x.Text2, languageId, storeScope, false, false);
                locale.Link2 = _localizationService.GetLocalizedSetting(nivoSliderSettings, (NivoSliderSettings x) => x.Link2, languageId, storeScope, false, false);
                locale.AltText2 = _localizationService.GetLocalizedSetting(nivoSliderSettings, (NivoSliderSettings x) => x.AltText2, languageId, storeScope, false, false);

                locale.Picture3Id = pic3Id;
                locale.Text3 = _localizationService.GetLocalizedSetting(nivoSliderSettings, (NivoSliderSettings x) => x.Text3, languageId, storeScope, false, false);
                locale.Link3 = _localizationService.GetLocalizedSetting(nivoSliderSettings, (NivoSliderSettings x) => x.Link3, languageId, storeScope, false, false);
                locale.AltText3 = _localizationService.GetLocalizedSetting(nivoSliderSettings, (NivoSliderSettings x) => x.AltText3, languageId, storeScope, false, false);

                locale.Picture4Id = pic4Id;
                locale.Text4 = _localizationService.GetLocalizedSetting(nivoSliderSettings, (NivoSliderSettings x) => x.Text4, languageId, storeScope, false, false);
                locale.Link4 = _localizationService.GetLocalizedSetting(nivoSliderSettings, (NivoSliderSettings x) => x.Link4, languageId, storeScope, false, false);
                locale.AltText4 = _localizationService.GetLocalizedSetting(nivoSliderSettings, (NivoSliderSettings x) => x.AltText4, languageId, storeScope, false, false);

                locale.Picture5Id = pic5Id;
                locale.Text5 = _localizationService.GetLocalizedSetting(nivoSliderSettings, (NivoSliderSettings x) => x.Text5, languageId, storeScope, false, false);
                locale.Link5 = _localizationService.GetLocalizedSetting(nivoSliderSettings, (NivoSliderSettings x) => x.Link5, languageId, storeScope, false, false);
                locale.AltText5 = _localizationService.GetLocalizedSetting(nivoSliderSettings, (NivoSliderSettings x) => x.AltText5, languageId, storeScope, false, false);
            });

            return View("~/Plugins/Widgets.NivoSlider/Views/Configure.cshtml", model);
        }

        [HttpPost]
        public IActionResult Configure(ConfigurationModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageWidgets))
                return AccessDeniedView();

            //load settings for a chosen store scope
            var storeScope = _storeContext.ActiveStoreScopeConfiguration;
            var nivoSliderSettings = _settingService.LoadSetting<NivoSliderSettings>(storeScope);

            //get previous picture identifiers
            var previousPictureIds = new[] 
            {
                int.Parse(nivoSliderSettings.Picture1Id),
                int.Parse(nivoSliderSettings.Picture2Id),
                int.Parse(nivoSliderSettings.Picture3Id),
                int.Parse(nivoSliderSettings.Picture4Id),
                int.Parse(nivoSliderSettings.Picture5Id)
            };

            nivoSliderSettings.Picture1Id = model.Picture1Id.ToString();
            nivoSliderSettings.Text1 = model.Text1;
            nivoSliderSettings.Link1 = model.Link1;
            nivoSliderSettings.AltText1 = model.AltText1;
            nivoSliderSettings.Picture2Id = model.Picture2Id.ToString();
            nivoSliderSettings.Text2 = model.Text2;
            nivoSliderSettings.Link2 = model.Link2;
            nivoSliderSettings.AltText2 = model.AltText2;
            nivoSliderSettings.Picture3Id = model.Picture3Id.ToString();
            nivoSliderSettings.Text3 = model.Text3;
            nivoSliderSettings.Link3 = model.Link3;
            nivoSliderSettings.AltText3 = model.AltText3;
            nivoSliderSettings.Picture4Id = model.Picture4Id.ToString();
            nivoSliderSettings.Text4 = model.Text4;
            nivoSliderSettings.Link4 = model.Link4;
            nivoSliderSettings.AltText4 = model.AltText4;
            nivoSliderSettings.Picture5Id = model.Picture5Id.ToString();
            nivoSliderSettings.Text5 = model.Text5;
            nivoSliderSettings.Link5 = model.Link5;
            nivoSliderSettings.AltText5 = model.AltText5;

            /* We do not clear cache after each setting update.
             * This behavior can increase performance because cached settings will not be cleared 
             * and loaded from database after each update */
            _settingService.SaveSettingOverridablePerStore(nivoSliderSettings, x => x.Picture1Id, model.Picture1Id_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(nivoSliderSettings, x => x.Text1, model.Text1_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(nivoSliderSettings, x => x.Link1, model.Link1_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(nivoSliderSettings, x => x.AltText1, model.AltText1_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(nivoSliderSettings, x => x.Picture2Id, model.Picture2Id_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(nivoSliderSettings, x => x.Text2, model.Text2_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(nivoSliderSettings, x => x.Link2, model.Link2_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(nivoSliderSettings, x => x.AltText2, model.AltText2_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(nivoSliderSettings, x => x.Picture3Id, model.Picture3Id_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(nivoSliderSettings, x => x.Text3, model.Text3_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(nivoSliderSettings, x => x.Link3, model.Link3_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(nivoSliderSettings, x => x.AltText3, model.AltText3_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(nivoSliderSettings, x => x.Picture4Id, model.Picture4Id_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(nivoSliderSettings, x => x.Text4, model.Text4_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(nivoSliderSettings, x => x.Link4, model.Link4_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(nivoSliderSettings, x => x.AltText4, model.AltText4_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(nivoSliderSettings, x => x.Picture5Id, model.Picture5Id_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(nivoSliderSettings, x => x.Text5, model.Text5_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(nivoSliderSettings, x => x.Link5, model.Link5_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(nivoSliderSettings, x => x.AltText5, model.AltText5_OverrideForStore, storeScope, false);

            //now clear settings cache
            _settingService.ClearCache();

            using (var enumerator = model.Locales.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    var c = enumerator.Current;
                    _localizationService.SaveLocalizedSetting(nivoSliderSettings, (NivoSliderSettings x) => x.Picture1Id, c.LanguageId, c.Picture1Id.ToString());
                    _localizationService.SaveLocalizedSetting(nivoSliderSettings, (NivoSliderSettings x) => x.Text1, c.LanguageId, c.Text1);
                    _localizationService.SaveLocalizedSetting(nivoSliderSettings, (NivoSliderSettings x) => x.Link1, c.LanguageId, c.Link1);
                    _localizationService.SaveLocalizedSetting(nivoSliderSettings, (NivoSliderSettings x) => x.AltText1, c.LanguageId, c.AltText1);

                    _localizationService.SaveLocalizedSetting(nivoSliderSettings, (NivoSliderSettings x) => x.Picture2Id, c.LanguageId, c.Picture2Id.ToString());
                    _localizationService.SaveLocalizedSetting(nivoSliderSettings, (NivoSliderSettings x) => x.Text2, c.LanguageId, c.Text2);
                    _localizationService.SaveLocalizedSetting(nivoSliderSettings, (NivoSliderSettings x) => x.Link2, c.LanguageId, c.Link2);
                    _localizationService.SaveLocalizedSetting(nivoSliderSettings, (NivoSliderSettings x) => x.AltText2, c.LanguageId, c.AltText2);

                    _localizationService.SaveLocalizedSetting(nivoSliderSettings, (NivoSliderSettings x) => x.Picture3Id, c.LanguageId, c.Picture3Id.ToString());
                    _localizationService.SaveLocalizedSetting(nivoSliderSettings, (NivoSliderSettings x) => x.Text3, c.LanguageId, c.Text3);
                    _localizationService.SaveLocalizedSetting(nivoSliderSettings, (NivoSliderSettings x) => x.Link3, c.LanguageId, c.Link3);
                    _localizationService.SaveLocalizedSetting(nivoSliderSettings, (NivoSliderSettings x) => x.AltText3, c.LanguageId, c.AltText3);

                    _localizationService.SaveLocalizedSetting(nivoSliderSettings, (NivoSliderSettings x) => x.Picture4Id, c.LanguageId, c.Picture4Id.ToString());
                    _localizationService.SaveLocalizedSetting(nivoSliderSettings, (NivoSliderSettings x) => x.Text4, c.LanguageId, c.Text4);
                    _localizationService.SaveLocalizedSetting(nivoSliderSettings, (NivoSliderSettings x) => x.Link4, c.LanguageId, c.Link4);
                    _localizationService.SaveLocalizedSetting(nivoSliderSettings, (NivoSliderSettings x) => x.AltText4, c.LanguageId, c.AltText4);

                    _localizationService.SaveLocalizedSetting(nivoSliderSettings, (NivoSliderSettings x) => x.Picture5Id, c.LanguageId, c.Picture5Id.ToString());
                    _localizationService.SaveLocalizedSetting(nivoSliderSettings, (NivoSliderSettings x) => x.Text5, c.LanguageId, c.Text5);
                    _localizationService.SaveLocalizedSetting(nivoSliderSettings, (NivoSliderSettings x) => x.Link5, c.LanguageId, c.Link5);
                    _localizationService.SaveLocalizedSetting(nivoSliderSettings, (NivoSliderSettings x) => x.AltText5, c.LanguageId, c.AltText5);
                }
            }

            //get current picture identifiers
            var currentPictureIds = new[]
            {
                int.Parse(nivoSliderSettings.Picture1Id),
                int.Parse(nivoSliderSettings.Picture2Id),
                int.Parse(nivoSliderSettings.Picture3Id),
                int.Parse(nivoSliderSettings.Picture4Id),
                int.Parse(nivoSliderSettings.Picture5Id)
            };

            //delete an old picture (if deleted or updated)
            foreach (var pictureId in previousPictureIds.Except(currentPictureIds))
            { 
                var previousPicture = _pictureService.GetPictureById(pictureId);
                if (previousPicture != null)
                    _pictureService.DeletePicture(previousPicture);
            }

            _notificationService.SuccessNotification(_localizationService.GetResource("Admin.Plugins.Saved"));
            return Configure();
        }
    }
}