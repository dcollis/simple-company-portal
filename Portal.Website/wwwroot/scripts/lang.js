import Vue from 'vue';
import VueI18n from 'vue-i18n';
import Cookies from 'js-cookie';
import en from '../languages/en';
import nl from '../languages/nl';
import moment from 'moment';

Vue.use(VueI18n);

// Ready translated locale messages
var words = {
    'en': en,
    'nl': nl
};


var GetLocale = function () {
    var cLocale = Cookies.get('locale');
    if (cLocale == null) {
        var lang = navigator.language || navigator.userLanguage;
        cLocale = lang[0] + lang[1];
    }
    moment.locale(cLocale);

    console.log(cLocale);
    return cLocale;
};

// Create VueI18n instance with options
export default new VueI18n({
    locale: GetLocale(), // set locale
    messages: words // set locale messages
});


