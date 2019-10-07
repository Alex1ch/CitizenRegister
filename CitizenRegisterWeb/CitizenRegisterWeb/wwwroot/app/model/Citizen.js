Ext.define('CitizenRegisterFront.model.Citizen', {
    extend: 'CitizenRegisterFront.model.Base',

    fields: [
        { name: 'id', type: 'int' },
        { name: 'name', type: 'string' },
        { name: 'surname', type: 'string' },
        { name: 'middleName', type: 'string' },
        { name: 'birthDate', type: 'date' }
    ]
});
