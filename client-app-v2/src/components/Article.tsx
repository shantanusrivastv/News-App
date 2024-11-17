// import React from 'react';

export interface IArticle {
	articleId: number;
	title: string;
	body: string;
	author: string;
	datePublished: string;
	articleReaction: UserReaction;
}

export enum UserReaction {
  Like,
  Dislike,
  None
}

// const Article: React.FC<IArticle> = ({ id, title, description, datePublished }) => {
// 	return (
// 		<div>
// 		<h2>{ title } </h2>
// 		< p > { description } </p>
// 		< small > { datePublished } </small>
// 		</div>
// 	);
// };

// export default Article;