import Vue from 'vue';
import App from '../calendar.vue';
import i18n from '../lang';

Vue.config.productionTip = false;

Vue.use(require('vue-moment'));

/* eslint-disable no-new */
new Vue({
    i18n,
    render: h => h(App),
    el: '#app'
});
