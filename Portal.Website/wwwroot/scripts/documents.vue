<template>
    <div>
        <navbar page="documents"></navbar>

        <div class="container">
            <div class="breadcrumbs" style="text-align: center">
                <nav aria-label="breadcrumb" role="navigation">
                    <ol class="breadcrumb">
                        <li class="breadcrumb-item" v-on:click="init()" style="text-transform:capitalize">
                            {{ $t("home") }}
                        </li>
                        <li class="breadcrumb-item" v-for="(breadcrumb, index) in breadcrumbs" v-on:click="moveUpTree(breadcrumb)" style="text-transform:capitalize">
                            {{ decodeURIComponent(breadcrumb) }}
                        </li>
                    </ol>
                </nav>
            </div>

            <div class="card" style="padding: 10px">
                <dx-data-grid :data-source="documents"
                              :show-borders="true"
                              :allow-column-resizing="true"
                              :word-wrap-enabled="true"
                              :hover-state-enabled="true"
                              @row-click="openDirectory"
                              @toolbar-preparing="onToolbarPreparing($event)"
                              :ref="documentsKey">
                    <dx-header-filter :visible="showHeaderFilter" />
                    <dx-paging :enabled="false" />
                    <dx-column cell-template="icon-template" width="38" />
                    <dx-column data-field="name" caption="Name" :calculate-cell-value="formatName" :sort-order="'asc'" />
                    <dx-column data-field="uploadDate" caption="Upload Date" :calculate-cell-value="formatDate" width="140" />
                    <template #icon-template="{ data }">
                        <i v-if="data.data.isDirectory" class="material-icons" data-toggle="tooltip" title="directory">folder</i>
                        <i v-else-if="data.data.name.endsWith('.pdf')" class="far fa-file-pdf fa-2x"></i>
                        <i v-else-if="data.data.name.endsWith('.docx') || data.data.name.endsWith('.docm') || data.data.name.endsWith('.dotx') || data.data.name.endsWith('.dotm') || data.data.name.endsWith('.docb')" class="far fa-file-word fa-2x"></i>
                        <i v-else-if="data.data.name.endsWith('.xlsx') || data.data.name.endsWith('.xlsm') || data.data.name.endsWith('.xltx') || data.data.name.endsWith('.xltm') || data.data.name.endsWith('.docb')" class="far fa-file-excel fa-2x"></i>
                        <i v-else class="far fa-file fa-2x"></i>
                    </template>
                </dx-data-grid>
            </div>
        </div>
        <portalFooter></portalFooter>
    </div>
</template>

<script>
    import navbar from './components/navbar.vue';
    import portalFooter from './components/footer.vue';
    import loadingWidget from './components/loading.vue';
    import {
        DxDataGrid, DxColumn,
        DxGrouping,
        DxGroupPanel,
        DxHeaderFilter,
        DxSearchPanel,
        DxFilterRow,
        DxFormat,
        DxSummary,
        DxGroupItem,
        DxTotalItem,
        DxPaging
    } from 'devextreme-vue/data-grid';

    var SortByType = function (a, b) {
        if (a.isDirectory) return -1;
        if (b.isDirectory) return 1;
        return 0;
    };

    export default {
        data() {
            this.loading = true;
            this.init();

            return {
                breadcrumbs: [],
                documents: [],
                pageSize: 20,
                loading: true,
                showHeaderFilter: true,
                documentsKey: 'documents-data-grid'
            }
        },
        methods: {
            init: function () {
                this.breadcrumbs = [];
                this.getDocuments();
            },
            getDocuments: function () {
                var self = this;
                var path = '';
                if (self.breadcrumbs.length > 0) {
                    path = "?path=" + encodeURIComponent(self.breadcrumbs.join('/'));
                }
                $.getJSON("api/documents/getdocuments" + path,
                    function (response) {
                        self.SetDocuments(response);
                        self.loading = false;
                    });
            },
            openDirectory: function (row) {
                let doc = row.data;
                var self = this;
                if (!doc.isDirectory) {
                    self.download(doc.prefix, doc.name)
                    return;
                }
                self.documentsGrid.beginCustomLoading('Loading...');
                var path = self.getFullPath(doc.name);
                $.getJSON("api/documents/getdocuments?path=" + encodeURIComponent(path),
                    function (response) {
                        self.breadcrumbs.push(doc.name);
                        self.SetDocuments(response);

                        self.documentsGrid.endCustomLoading();
                    });

            },
            moveUpTree: function (name) {
                while (this.breadcrumbs[this.breadcrumbs.length - 1] !== name) {
                    this.breadcrumbs.pop();
                }
                this.getDocuments();
            },
            download: function (prefix, doc) {
                let self = this;
                let path = prefix + '/' + doc;
                window.open('api/documents/download?file=' + encodeURIComponent(path), '_blank');
            },
            getFullPath: function (name) {
                var self = this;
                var path = self.breadcrumbs.join('/');
                if (path !== '') {
                    path += '/';
                }
                path += name;
                return path;
            },
            SetDocuments: function (documents) {
                documents.sort(SortByType);
                this.documents = documents;
            },
            formatName: function (row) {
                return decodeURIComponent(row.name);
            },
            formatDate: function (row) {
                if (row.isDirectory) {
                    return '';
                }

                return row.uploadDate;
            },
            search: function (e) {
                let self = this;
                self.documentsGrid.beginCustomLoading('Loading...');
                $.getJSON("api/documents/search?pattern=" + encodeURIComponent(e.value),
                    function (response) {
                        self.SetDocuments(response);

                        self.documentsGrid.endCustomLoading();
                    });
            },
            onToolbarPreparing: function (e) {
                let self = this;
                e.toolbarOptions.items.unshift({
                        location: 'after',
                        widget: 'dxTextBox',
                    options: {
                        placeholder: self.$t('Search') + '...',
                        onValueChanged: self.search
                    }
                });
            }
        },
        computed: {
            documentsGrid: function () {
                let self = this;
                return self.$refs[self.documentsKey].instance;
            }
        },
        components: {
            navbar,
            portalFooter,
            loadingWidget,
            DxDataGrid, DxColumn,
            DxGrouping,
            DxGroupPanel,
            DxHeaderFilter,
            DxSearchPanel,
            DxFilterRow,
            DxFormat,
            DxSummary,
            DxGroupItem,
            DxTotalItem,
            DxPaging
        }
    }
</script>


<style lang="scss">
    @import url("https://fonts.googleapis.com/css?family=Roboto:300,400,500,700|Roboto+Slab:400,700|Material+Icons");
    @import url("https://cdn3.devexpress.com/jslib/19.1.6/css/dx.common.css");
    @import url("https://cdn3.devexpress.com/jslib/19.1.6/css/dx.light.css");

    .card [class*="header-"] {
        color: #959595 !important;
    }

    .dx-row, .breadcrumb-item {
        cursor: pointer;
    }
</style>