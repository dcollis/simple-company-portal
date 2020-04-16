<template>
    <footer class="footer footer-default" id="mainFooter">
        <div class="container">
            <div class="row">
                <div class="col">

                    <ul class="pagination">
                        <li class="page-item"><a class="page-link" v-on:click="changeLang('en')">EN</a></li>
                        <li class="page-item"><a class="page-link disabled">|</a></li>
                        <li class="page-item"><a class="page-link" v-on:click="changeLang('nl')">NL</a></li>
                    </ul>
                </div>
                <div class="col">
                    <div class="copyright float-right">
                        &copy;
                        {{ date }}
                        <a href="https://www.duetgroup.net" target="blank">DuetGroup</a>
                    </div>
                </div>
            </div>
        </div>
    </footer>
</template>

<script>
    export default {
        name: 'mainFooter',
        data() {
            return {
                date: new Date().getFullYear() 
            }
        },
        methods: {
            changeLang: function (lang) {
                var self = this;
                self.$i18n.locale = lang;
                Cookies.set('locale', lang);
                self.$moment.locale(lang)
                location.reload();
            }
        }
    };
</script>