Ext.define('CitizenRegisterFront.store.Citizen', {
    extend: 'Ext.data.Store',

    alias: 'store.citizen',

    model: 'CitizenRegisterFront.model.Citizen',

    data: { items: []},

    proxy: {
        type: 'memory',
        reader: {
            type: 'json',
            rootProperty: 'items'
        }
    }
});
