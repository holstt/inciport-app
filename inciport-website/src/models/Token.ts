export default class Token {
	value: string;
	expiration: string;

	constructor(value: string, expiration: string) {
		this.value = value;
		this.expiration = expiration;
	}
}
