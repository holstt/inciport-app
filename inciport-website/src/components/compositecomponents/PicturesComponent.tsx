import { FC, useContext, useEffect, useState } from 'react';
import { Context } from '../../Context';
import { APIClient } from '../../services/ApiClient';
import PictureComponent from '../basecomponents/PictureComponent';
import '../../css/PicturesStyles.css';

export const PicturesComponent: FC = () => {
	const { incidentReport } = useContext(Context);
	const [images, setImages] = useState<string[]>([]);

	useEffect(() => {
		const getImages = async () => {
			let tempImages: string[] = [];
			for (let i = 0; i < incidentReport.imageUrls.length; i++) {
				try {
					const response = await APIClient.getImage(incidentReport.imageUrls[i]);
					tempImages.push(response);
				} catch (error: any) {
					tempImages.push('');
					console.log(`Couldn't get the image ${i}`);
					console.log(error.toString());
				}
			}
			setImages(tempImages);
		};
		getImages();
	}, []);

	return (
		<div id='Pictures' className='PicturesContainer Column'>
			<h4 className='OverviewMessage'>Pictures:</h4>
			{images.map((image, index) => {
				return <PictureComponent key={index} image={image} />;
			})}
		</div>
	);
};

export default PicturesComponent;
