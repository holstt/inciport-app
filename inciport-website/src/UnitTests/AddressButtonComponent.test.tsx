import { mount } from 'enzyme';
import AddressButtonsComponent from '../components/compositecomponents/AddressButtonsComponent';

describe('AddressButtonComponent', () => {
    const onSave = jest.fn();
    const onCancel = jest.fn();
    const onDelete = jest.fn();

    afterEach(() => {
        onSave.mockClear();
        onCancel.mockClear();
        onDelete.mockClear();
    })

    it("Should be able to receive a generic function, which can be called on Save Changes button's onClick event", () => {

        const isIncidentReportUpdated = true;

        const component = mount(<AddressButtonsComponent onSave={onSave}
            onCancel={onCancel}
            onDelete={onDelete}
            disabledCondition={!isIncidentReportUpdated} />);

        component.find('#SaveButton').simulate('click');
        expect(onSave).toHaveBeenCalled();
        expect(onCancel).not.toHaveBeenCalled();
        expect(onDelete).not.toHaveBeenCalled();
    })

    it("Should be able to receive a generic function, which can be called on Cancel button's onClick event", () => {
        const isIncidentReportUpdated = true;

        const component = mount(<AddressButtonsComponent onSave={onSave}
            onCancel={onCancel}
            onDelete={onDelete}
            disabledCondition={!isIncidentReportUpdated} />);

        component.find('#CancelButton').simulate('click');
        expect(onSave).not.toHaveBeenCalled();
        expect(onCancel).toHaveBeenCalled();
        expect(onDelete).not.toHaveBeenCalled();
    })

    it("Should be able to receive a generic function, which can be called on Delete button's onClick event", () => {
        const isIncidentReportUpdated = true;

        const component = mount(<AddressButtonsComponent onSave={onSave}
            onCancel={onCancel}
            onDelete={onDelete}
            disabledCondition={!isIncidentReportUpdated} />);

        component.find('#DeleteButton').simulate('click');
        expect(onSave).not.toHaveBeenCalled();
        expect(onCancel).not.toHaveBeenCalled();
        expect(onDelete).toHaveBeenCalled();
    })
})
