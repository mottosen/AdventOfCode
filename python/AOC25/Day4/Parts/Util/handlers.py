def handle_grid(grid, x_max, y_max, remove=False):
    # the coordinates accessible by forklift
    forklift_accees = []

    # keep recent rows in scope/memory
    last_row = []
    cur_row = grid[0]

    for y in range(y_max):
        next_row = [] if y + 1 == y_max else grid[y + 1]

        for x in range(x_max):
            # there must be a paper roll to access
            if cur_row[x] != "@":
                continue

            # find neighbors in bounds
            neighbors = (
                (
                    []
                    if y == 0
                    else (last_row[x : x + 2] if x == 0 else last_row[x - 1 : x + 2])
                )
                + ([cur_row[x + 1]] if x == 0 else [cur_row[x - 1], cur_row[x + 1]])
                + (
                    []
                    if y + 1 == y_max
                    else (next_row[x : x + 2] if x == 0 else next_row[x - 1 : x + 2])
                )
            )

            # check whether this paper roll is accessible
            paper_rolls = list(filter(lambda neighbor: neighbor == "@", neighbors))
            if len(paper_rolls) < 4:
                forklift_accees.insert(0, (y, x))
                if remove:
                    cur_row[x] = "x"

        last_row = cur_row
        cur_row = next_row

    return forklift_accees
