import Category from './Category';

export default class ChosenMainCategory extends Category {
	chosenSubCategory: Category | null;

	constructor(id: number, title: string, chosenSubCategory: Category | null) {
		super(id, title);
		this.chosenSubCategory = chosenSubCategory;
	}
}
