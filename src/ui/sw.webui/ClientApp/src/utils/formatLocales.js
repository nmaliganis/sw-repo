import { localizationResources } from "./locales";

let transformedResources = {
	en: {},
	el: {}
};

// Handler that shapes locales to be loaded for devextreme's localization tools
for (const resourceKey in localizationResources) {
	transformedResources.el[resourceKey] = localizationResources[resourceKey].el;

	transformedResources.en[resourceKey] = localizationResources[resourceKey].en;
}

export default transformedResources;
