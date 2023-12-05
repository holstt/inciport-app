import Category from './Category';

export default class MainCategoryOptions extends Category {
	subCategories: Category[];

	constructor(id: number, title: string, subCategories: Category[]) {
		super(id, title);
		this.subCategories = subCategories;
	}
}
