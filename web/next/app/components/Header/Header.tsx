import { AppBar, Toolbar, Typography } from '@mui/material'
import Link from 'next/link'
import ThemeSwitch from '../ThemeSwitch/ThemeSwitch'

const Header = () => (
  <AppBar position={'static'}>
    <Toolbar>
        <Typography variant={'h6'} component={'div'} sx={{ flexGrow: 1 }} className={'cursor-pointer'}>
          <Link href='/' passHref>Moody Metrics</Link>
        </Typography>
      <ThemeSwitch />
    </Toolbar>
  </AppBar>
)

export default Header