<template>
    <div class="card">
        <div class="card-body" id="documents">
            <h4 class="card-title">{{ $t("documents") }}</h4>
            <h6 class="card-subtitle mb-2 text-muted">{{ $t("latest") }}</h6>
            <div>

                <loadingAnimation v-if="loading"></loadingAnimation>
                <ul v-if="!loading" class="list-group list-group-flush" v-for="doc in docs">
                    <li class="list-group-item material-item">
                        <div style="width:100%">
                            <div style="width:50px; height:100%">
                                <a class="nav-link" style="vertical-align: middle; float: left; text-align: center;width:50px; height:100%" v-on:click="download(doc)">
                                    <i class="material-icons" data-toggle="tooltip" title="Download">vertical_align_bottom</i>
                                </a>
                            </div>
                            <div style="width:100%">
                                <span style="word-break: break-word;width:50px; height:100%">{{ decodeURIComponent(doc.name) }}</span>
                            </div>
                        </div>

                    </li>
                </ul>
            </div>
        </div>
    </div>
</template>

<script>
    import loadingAnimation from '../../loading.vue';

    export default {
        data() {
            return {
                docs: [],
                loading: true
            }
        },
        created() {
            this.getDocuments();
        },
        methods: {
            download: function (doc) {
                var path = '';
                if (doc.prefix !== null) {
                    path += doc.prefix + '/';
                }
                path += doc.name;
                window.open('api/documents/download?file=' + path, '_blank');
            },
            getDocuments: function () {
                var self = this;
                self.loading = true;
                $.getJSON(
                    'api/documents/getlimiteddocuments?numItems=5',
                    function (response) {
                        self.docs = response;
                        self.loading = false;
                    });
            }
        },
        components: {
            loadingAnimation
        }
    }
</script>

<style lang="scss">
@import url("https://fonts.googleapis.com/css?family=Roboto:300,400,500,700|Roboto+Slab:400,700|Material+Icons");
</style>