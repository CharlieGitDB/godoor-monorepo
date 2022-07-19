import type { NextPage } from 'next'
import Head from 'next/head'

const Home: NextPage = () => {
  return (
    <div>
      <Head>
        <title>Godoor</title>
        <meta name="description" content="Godoor" />
        <link rel="icon" href="/favicon.ico" />
      </Head>

      <main>
        <span className={'underline'}>Hello</span>
      </main>
    </div>
  )
}

export default Home
