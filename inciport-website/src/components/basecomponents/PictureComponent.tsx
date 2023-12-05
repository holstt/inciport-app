import { FC, useState } from 'react';
import errorImage from '../../assets/errorImage.png';
import '../../css/PicturesStyles.css';

interface Props {
    image: string;
}

export const PictureComponent: FC<Props> = (props) => {
    const [image] = useState(() => {
        return props.image === '' ? errorImage : 'data:image/jpeg;base64,'+props.image;
    });

	return (
        <img src={image} alt=''></img>
	);
};

export default PictureComponent;
