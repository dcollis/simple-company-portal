import Vue from 'vue';
import App from '../people.vue';
import i18n from '../lang';

Vue.config.productionTip = false;


/* eslint-disable no-new */
new Vue({
    i18n,
    render: h => h(App),
    el: '#app'
});
