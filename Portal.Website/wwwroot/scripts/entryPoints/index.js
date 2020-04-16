import Vue from 'vue';
import App from '../dashboard.vue';
import i18n from '../lang';

Vue.config.productionTip = false;

const moment = require('moment');

Vue.use(require('vue-moment'), {
    moment
});

console.log(Vue.moment().locale());

/* eslint-disable no-new */
new Vue({
    i18n,
    render: h => h(App),
    el: '#app'
});
