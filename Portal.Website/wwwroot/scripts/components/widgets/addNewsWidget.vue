<template>
    <a id="addArticle" v-if="isAuthor"
       href="/addnews.html"
       class="btn btn-primary btn-fab btn-round"
       style="position: fixed; bottom: 10px; right: 10px;"
       data-toggle="tooltip" title="Add Article">
        <i class="material-icons">add</i>
    </a>
</template>

<script>
    import { get } from '../../utils/communication';

    export default {
        data() {
            return {
                isAuthor: false
            };
        },
        created() {
            this.init();
        },
        methods: {
            init: function () {
                var self = this;
                get('api/roles/isauthor',
                    function (response) {
                        self.isAuthor = response;
                    });
            }
        }
    }
</script>

<style lang="scss">
    @import url("https://fonts.googleapis.com/css?family=Roboto:300,400,500,700|Roboto+Slab:400,700|Material+Icons");
</style>