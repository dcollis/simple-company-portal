<template>
    <div>
        <navbar></navbar>
        <loadingWidget v-if="loading"></loadingWidget>
        <div v-if="!loading" class="container card" style="padding-top: 20px; padding-bottom: 20px; height: 100%">
            <h3 style="text-align: center;">Create a New Article</h3>
            <div class="form-group">
                <label>Title</label>
                <input class="form-control" placeholder="Enter a Title" v-model="title">
                <small id="titleHelp" class="form-text text-muted">A title for your new article.</small>
            </div>
            <ckeditor :editor="editor" v-model="editorData" :config="editorConfig"></ckeditor>
            <br />
            <button class="btn btn-primary btn-round" style="width: 150px;" v-on:click="save()">
                <i class="material-icons">save</i> Save
            </button>
        </div>

        <portalFooter></portalFooter>
    </div>
</template>

<script>

    import navbar from './components/navbar.vue';
    import portalFooter from './components/footer.vue';
    import loadingWidget from './components/loading.vue';
    import { get, post } from './utils/communication';
    import CKEditor from '@ckeditor/ckeditor5-vue';

    export default {
        data() {
            this.getUserType();
            return {
                isAuthor: false,
                title: "",
                editor: ClassicEditor,
                editorData: "",
                editorConfig: {
                    // The configuration of the editor.

                },
                loading: false
            }
        },
        methods: {
            save: function () {
                var self = this;
                self.loading = true;
                post(
                    "/api/news/create",
                    JSON.stringify({
                        title: this.title,
                        body: this.editorData
                    }),
                    function () {
                        window.open("/news.html", "_self");
                    }).then(
                        function () {
                            window.open("/news.html", "_self");
                        });
            },
            getUserType: function () {
                var self = this;
                get('/api/roles/isauthor',
                    function (isAuthor) {
                        if (!isAuthor) {
                            window.location.href = encodeURI("error.html#You do not permission to view this page.");
                        } else {
                            self.isAuthor = isAuthor;
                        }
                    });
            }
        },
        components: {
            navbar,
            portalFooter,
            loadingWidget,
            ckeditor: CKEditor.component
        }
    }
</script>


<style lang="scss">
    @import url("https://fonts.googleapis.com/css?family=Roboto:300,400,500,700|Roboto+Slab:400,700|Material+Icons");
</style>